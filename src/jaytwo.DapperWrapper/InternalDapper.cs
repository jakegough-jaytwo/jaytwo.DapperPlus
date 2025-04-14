using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Dapper;
using OpenTracing;

namespace jaytwo.DapperWrapper;

internal class InternalDapper
{
    public InternalDapper(
        IsolationLevel transactionIsolationLevel,
        int commandTimeoutSeconds,
        int cancellationTimeoutSeconds,
        ITracer? tracer = default)
    {
        TransactionIsolationLevel = transactionIsolationLevel;
        CommandTimeoutSeconds = commandTimeoutSeconds;
        CancellationTimeoutSeconds = cancellationTimeoutSeconds;
        Tracer = tracer;
    }

    public IsolationLevel TransactionIsolationLevel { get; }

    public int CancellationTimeoutSeconds { get; }

    public int CommandTimeoutSeconds { get; }

    public ITracer? Tracer { get; }

    public static async Task OpenConnectionIfClosedAsync(DbConnection connection, ITracer? tracer = default, string? tracerScopeNamePrefix = default, CancellationToken cancellationToken = default)
    {
        if (connection.State == ConnectionState.Closed)
        {
            using (tracer?.BuildSpan(tracerScopeNamePrefix + "OpenConnection").StartActive())
            {
                await connection.OpenAsync(cancellationToken);
            }
        }
    }

    public async Task<T> RunWithCancellationTokenAsync<T>(
        Func<DbConnection> connectionFactory,
        Func<DbConnection, CommandDefinition, Task<T>> queryDelegate,
        string commandText,
        object? parameters,
        DbTransaction? transaction,
        int? commandTimeoutSeconds,
        CommandType? commandType,
        CommandFlags flags,
        int? cancellationTimeoutSeconds,
        CancellationToken cancellationToken)
    {
        using (Tracer?.BuildSpan(GetType().Name + "." + nameof(RunWithCancellationTokenAsync)).StartActive())
        using (var timeoutCancellationTokenSource = GetCancellationTimeoutTokenSource(cancellationTimeoutSeconds))
        using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token))
        {
            var commandDefinition = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeout: commandTimeoutSeconds ?? CommandTimeoutSeconds,
                commandType: commandType,
                flags: flags,
                cancellationToken: linkedTokenSource.Token);

            var connection = transaction?.Connection ?? connectionFactory();
            try
            {
                return await queryDelegate.Invoke(connection, commandDefinition);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    await connection.DisposeAsync();
                }
            }
        }
    }

#if NET5_0_OR_GREATER
    public IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        Func<DbConnection> connectionFactory,
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default)
    {
        // cancellationToken is not needed in the signature for this method because when WithCancellation()
        //   is used on the resulting IAsyncEnumerable, it will be bound to the `Impl`'s CancellationToken
        //   since it's decorated with [EnumeratorCancellation]... it's all very confusing but it's how
        //   Dapper does it... I stole the pattern from them
        // (and yes, it will cancel the underlying query if necessary, not just the enumeration of the results)
        // (and yes, it will be linked to the timeoutCancellationTokenSource's Token so if a cancellation
        //   is requested from either the WithCancellation()'s token or the timeoutCancellationTokenSource's
        //   Token, it will be honored)

        var tracerScopeName = GetType().Name + "." + nameof(QueryUnbufferedAsync);
        using (Tracer?.BuildSpan(tracerScopeName).StartActive())
        using (var timeoutCancellationTokenSource = GetCancellationTimeoutTokenSource(cancellationTimeoutSeconds))
        {
            var connection = transaction?.Connection ?? connectionFactory();

            return Impl(
                connection: connection,
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeoutSeconds: commandTimeoutSeconds ?? CommandTimeoutSeconds,
                commandType: commandType,
                tracer: Tracer,
                tracerScopeNamePrefix: tracerScopeName + ".",
                cancellationToken: timeoutCancellationTokenSource.Token);
        }

        static async IAsyncEnumerable<T> Impl(
            DbConnection connection,
            string commandText,
            object? parameters,
            DbTransaction? transaction,
            int? commandTimeoutSeconds,
            CommandType? commandType,
            ITracer? tracer,
            string tracerScopeNamePrefix,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            // just making sure opening the connection doesn't throw off the telemetry for the first row time
            await OpenConnectionIfClosedAsync(connection, tracer, tracerScopeNamePrefix, cancellationToken);

            var firstRowTracerScope = tracer?.BuildSpan(tracerScopeNamePrefix + "." + "FirstRow").StartActive();

            try
            {
                var rows = connection.QueryUnbufferedAsync<T>(
                    commandText,
                    parameters,
                    transaction,
                    commandTimeoutSeconds,
                    commandType)
                    .WithCancellation(cancellationToken);

                var enumerator = rows.GetAsyncEnumerator();

                if (await enumerator.MoveNextAsync())
                {
                    firstRowTracerScope?.Dispose();

                    yield return enumerator.Current;

                    while (await enumerator.MoveNextAsync())
                    {
                        yield return enumerator.Current;
                    }
                }
            }
            finally
            {
                firstRowTracerScope?.Dispose();

                if (transaction == null)
                {
                    await connection.DisposeAsync();
                }
            }
        }
    }
#endif

    private CancellationTokenSource GetCancellationTimeoutTokenSource(int? cancellationTimeoutSeconds)
        => new CancellationTokenSource(TimeSpan.FromSeconds(cancellationTimeoutSeconds ?? CancellationTimeoutSeconds));
}
