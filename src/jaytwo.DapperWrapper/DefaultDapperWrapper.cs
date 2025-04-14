using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Dapper;
using OpenTracing;
using static Dapper.SqlMapper;

namespace jaytwo.DapperWrapper;

public class DefaultDapperWrapper
    : IDapperWrapper
{
    public const IsolationLevel DefaultTransactionIsolationLevel = IsolationLevel.Unspecified;
    public const int DefaultCommandTimeoutSeconds = 30;
    public const int DefaultCancellationTimeoutSeconds = 45;

    private readonly Func<DbConnection> _connectionFactory;

    public DefaultDapperWrapper(
        Func<DbConnection> connectionFactory,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds,
        ITracer? tracer = default)
    {
        _connectionFactory = connectionFactory;
        TransactionIsolationLevel = transactionIsolationLevel ?? DefaultTransactionIsolationLevel;
        CommandTimeoutSeconds = commandTimeoutSeconds ?? DefaultCommandTimeoutSeconds;
        CancellationTimeoutSeconds = cancellationTimeoutSeconds ?? DefaultCancellationTimeoutSeconds;
        Tracer = tracer;
    }

    public IsolationLevel TransactionIsolationLevel { get; }

    public virtual int CancellationTimeoutSeconds { get; }

    public virtual int CommandTimeoutSeconds { get; }

    protected ITracer? Tracer { get; }

    public virtual async Task<object> HealthCheckAsync(CancellationToken cancellationToken = default)
    {
        // TODO: make this easier to override

        using var connection = CreateConnection();
        if (connection.ConnectionString.StartsWith("TODO"))
        {
            return "Disabled";
        }

        var result = new Dictionary<string, object>();

        try
        {
            var nowTime = await ExecuteScalarAsync<DateTime>("SELECT now()");
            result["serverTime"] = nowTime.ToString("O");
        }
        catch (Exception ex)
        {
            var healthCheckException = new Exception(ex.Message, ex);
            healthCheckException.Data.Add(nameof(result), result);
            throw healthCheckException;
        }

        return result;
    }

    public virtual DbConnection CreateConnection()
        => _connectionFactory.Invoke();

    public virtual async Task RunInTransactionAsync(Func<DbTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default)
    {
        using (var connection = CreateConnection())
        using (var transaction = await OpenTransactionAsync(connection, isolationLevel, cancellationToken))
        {
            await callback.Invoke(transaction);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    public virtual async Task CommitTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default)
    {
        using (Tracer?.BuildSpan(GetType().Name + ".CommitTransaction").StartActive())
        {
            await transaction.CommitAsync(cancellationToken);
        }
    }

    public virtual async Task RollbackTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default)
    {
        using (Tracer?.BuildSpan(GetType().Name + ".RollbackTransaction").StartActive())
        {
            await transaction.RollbackAsync(cancellationToken);
        }
    }

    public virtual async Task<int> ExecuteAsync(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        CancellationToken cancellationToken = default)
        => await RunWithCancellationTokenAsync(
             queryDelegate: (conn, comm) => conn.ExecuteAsync(comm),
             commandText: commandText,
             parameters: parameters,
             transaction: transaction,
             commandTimeoutSeconds: commandTimeoutSeconds,
             commandType: commandType,
             flags: CommandFlags.Buffered,
             cancellationTimeoutSeconds: cancellationTimeoutSeconds,
             cancellationToken: cancellationToken);

    public async Task<T?> ExecuteScalarAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        CancellationToken cancellationToken = default)
        => await RunWithCancellationTokenAsync<T?>(
            queryDelegate: (conn, comm) => conn.ExecuteScalarAsync<T?>(comm),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            flags: CommandFlags.Buffered,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

    public async Task<IList<T>> QueryAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        T? prototype = default,
        CancellationToken cancellationToken = default)
        => await RunWithCancellationTokenAsync<IList<T>>(
            queryDelegate: async (conn, comm) => (await conn.QueryAsync<T>(comm)).ToList(),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            flags: CommandFlags.Buffered,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

    public async Task<GridReader> QueryMultipleAsync(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        CancellationToken cancellationToken = default)
        => await RunWithCancellationTokenAsync<GridReader>(
            queryDelegate: async (conn, comm) => (await conn.QueryMultipleAsync(comm)),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            flags: CommandFlags.Buffered,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

    public async Task<T?> QuerySingleOrDefaultAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        T? prototype = default,
        CancellationToken cancellationToken = default)
        => await RunWithCancellationTokenAsync<T?>(
            queryDelegate: (conn, comm) => conn.QuerySingleOrDefaultAsync<T>(comm),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            flags: CommandFlags.Buffered,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

    public async Task<T> QuerySingleAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        T? prototype = default,
        CancellationToken cancellationToken = default)
        => await RunWithCancellationTokenAsync<T>(
            queryDelegate: (conn, comm) => conn.QuerySingleAsync<T>(comm),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            flags: CommandFlags.Buffered,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

#if NET5_0_OR_GREATER
    public virtual IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        int? cancellationTimeoutSeconds = default,
        CommandType? commandType = default,
        T? prototype = default)
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
            var connection = transaction?.Connection ?? CreateConnection();

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

    protected static async Task OpenConnectionIfClosedAsync(DbConnection connection, ITracer? tracer = default, string? tracerScopeNamePrefix = default, CancellationToken cancellationToken = default)
    {
        if (connection.State == ConnectionState.Closed)
        {
            using (tracer?.BuildSpan(tracerScopeNamePrefix + "OpenConnection").StartActive())
            {
                await connection.OpenAsync(cancellationToken);
            }
        }
    }

    protected virtual async Task OpenConnectionIfClosedAsync(DbConnection connection, CancellationToken cancellationToken)
        => await OpenConnectionIfClosedAsync(connection, Tracer, GetType().Name + ".", cancellationToken);

    protected virtual async Task<DbTransaction> OpenTransactionAsync(DbConnection connection, IsolationLevel? isolationLevel, CancellationToken cancellationToken)
    {
        using (Tracer?.BuildSpan(GetType().Name + ".BeginTransaction").StartActive())
        {
            await OpenConnectionIfClosedAsync(connection, cancellationToken);

            return await connection.BeginTransactionAsync(isolationLevel ?? TransactionIsolationLevel, cancellationToken);
        }
    }

    private async Task<T> RunWithCancellationTokenAsync<T>(
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

            var connection = transaction?.Connection ?? CreateConnection();
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

    private CancellationTokenSource GetCancellationTimeoutTokenSource(int? cancellationTimeoutSeconds)
        => new CancellationTokenSource(TimeSpan.FromSeconds(cancellationTimeoutSeconds ?? CancellationTimeoutSeconds));
}
