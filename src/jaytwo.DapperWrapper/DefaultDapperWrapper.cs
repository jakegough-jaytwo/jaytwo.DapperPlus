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

    internal DefaultDapperWrapper(Func<DbConnection> connectionFactory)
        : this(connectionFactory, default, default, default, default)
    {
    }

    public IsolationLevel TransactionIsolationLevel { get; }

    public int CancellationTimeoutSeconds { get; }

    public int CommandTimeoutSeconds { get; }

    protected ITracer? Tracer { get; }

    public virtual async Task<object> HealthCheckAsync(CancellationToken cancellationToken = default)
        => await new DatabaseHealthCheck<DefaultDapperWrapper>(this).HealthCheckAsync(cancellationToken);

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
        => await ExecuteAsync(
            BuildDapperCommandContext(
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeoutSeconds: commandTimeoutSeconds,
                commandType: commandType,
                cancellationTimeoutSeconds: cancellationTimeoutSeconds),
            cancellationToken);

    public virtual async Task<T?> ExecuteScalarAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        CancellationToken cancellationToken = default)
        => await ExecuteScalarAsync<T>(
            BuildDapperCommandContext(
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeoutSeconds: commandTimeoutSeconds,
                commandType: commandType,
                cancellationTimeoutSeconds: cancellationTimeoutSeconds),
            cancellationToken);

    public virtual async Task<IList<T>> QueryAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        T? prototype = default,
        CancellationToken cancellationToken = default)
        => await QueryAsync<T>(
                BuildDapperCommandContext(
                    commandText: commandText,
                    parameters: parameters,
                    transaction: transaction,
                    commandTimeoutSeconds: commandTimeoutSeconds,
                    commandType: commandType,
                    cancellationTimeoutSeconds: cancellationTimeoutSeconds),
                cancellationToken);

    public virtual async Task<T?> QuerySingleOrDefaultAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        T? prototype = default,
        CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<T>(
            BuildDapperCommandContext(
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeoutSeconds: commandTimeoutSeconds,
                commandType: commandType,
                cancellationTimeoutSeconds: cancellationTimeoutSeconds),
            cancellationToken);

    public virtual async Task<T> QuerySingleAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        T? prototype = default,
        CancellationToken cancellationToken = default)
        => await QuerySingleAsync<T>(
            BuildDapperCommandContext(
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeoutSeconds: commandTimeoutSeconds,
                commandType: commandType,
                cancellationTimeoutSeconds: cancellationTimeoutSeconds),
            cancellationToken);

    public virtual async Task<GridReader> QueryMultipleAsync(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        CancellationToken cancellationToken = default)
        => await QueryMultipleAsync(
            BuildDapperCommandContext(
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeoutSeconds: commandTimeoutSeconds,
                commandType: commandType,
                cancellationTimeoutSeconds: cancellationTimeoutSeconds),
            cancellationToken);

#if NET5_0_OR_GREATER
    public virtual IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        T? prototype = default)
        => QueryUnbufferedAsync<T>(
            BuildDapperCommandContext(
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeoutSeconds: commandTimeoutSeconds,
                commandType: commandType,
                cancellationTimeoutSeconds: cancellationTimeoutSeconds));
#endif

    internal static async Task OpenConnectionIfClosedAsync(DbConnection connection, ITracer? tracer = default, string? tracerScopeNamePrefix = default, CancellationToken cancellationToken = default)
    {
        if (connection.State == ConnectionState.Closed)
        {
            using (tracer?.BuildSpan(tracerScopeNamePrefix + ".OpenConnection").StartActive())
            {
                await connection.OpenAsync(cancellationToken);
            }
        }
    }

    internal virtual async Task<int> ExecuteAsync(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.ExecuteAsync(comm),
            context,
            CommandFlags.Buffered,
            cancellationToken);

    internal virtual async Task<T?> ExecuteScalarAsync<T>(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.ExecuteScalarAsync<T>(comm),
            context,
            CommandFlags.Buffered,
            cancellationToken);

    internal virtual async Task<IList<T>> QueryAsync<T>(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => (await conn.QueryAsync<T>(comm)).ToList(),
            context,
            CommandFlags.Buffered,
            cancellationToken);

    internal virtual async Task<T?> QuerySingleOrDefaultAsync<T>(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.QuerySingleOrDefaultAsync<T>(comm),
            context,
            CommandFlags.Buffered,
            cancellationToken);

    internal virtual async Task<T> QuerySingleAsync<T>(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.QuerySingleAsync<T>(comm),
            context,
            CommandFlags.Buffered,
            cancellationToken);

    internal virtual async Task<GridReader> QueryMultipleAsync(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.QueryMultipleAsync(comm),
            context,
            CommandFlags.Buffered,
            cancellationToken);

    internal virtual async Task<T> RunWithCancellationTokenAsync<T>(
        Func<DbConnection, CommandDefinition, Task<T>> queryDelegate,
        DapperCommandContext context,
        CommandFlags commandFlags,
        CancellationToken cancellationToken)
    {
        var tracerScopeName = GetType().Name + "." + nameof(RunWithCancellationTokenAsync);
        using var tracerScope = Tracer?.BuildSpan(tracerScopeName).StartActive();

        // just making sure opening the connection so we have the telemetry if the operation includes opening the connection
        var connection = context.Transaction?.Connection ?? _connectionFactory();
        await OpenConnectionIfClosedAsync(connection, Tracer, tracerScopeName, cancellationToken);

        using (var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(context.CancellationTimeoutSeconds)))
        using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token))
        {
            // we build up our own command definition solely to add a cancellationToken
            var commandDefinition = context.ToDapperCommandDefinition(commandFlags, linkedTokenSource.Token);

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
                if (context.Transaction == null)
                {
                    await connection.DisposeAsync();
                }
            }
        }
    }

#if NET5_0_OR_GREATER
    internal virtual IAsyncEnumerable<T> QueryUnbufferedAsync<T>(DapperCommandContext context)
        => QueryUnbufferedAsync<T>(
            queryDelegate: conn => conn.QueryUnbufferedAsync<T>(
                context.CommandText,
                context.Parameters,
                context.Transaction,
                context.CommandTimeoutSeconds,
                context.CommandType),
            context: context);

    internal virtual async IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        Func<DbConnection, IAsyncEnumerable<T>> queryDelegate,
        DapperCommandContext context,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var tracerScopeName = GetType().Name + "." + nameof(QueryUnbufferedAsync);
        using var tracerScope = Tracer?.BuildSpan(tracerScopeName).StartActive();

        // just making sure opening the connection doesn't throw off the telemetry for the first row time
        var connection = context.Transaction?.Connection ?? _connectionFactory();
        await OpenConnectionIfClosedAsync(connection, Tracer, tracerScopeName, cancellationToken);

        using var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(context.CancellationTimeoutSeconds));
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token);
        var firstRowTracerScope = Tracer?.BuildSpan(tracerScopeName + ".FirstRow").StartActive();
        try
        {
            var rows = queryDelegate(connection).WithCancellation(linkedTokenSource.Token);

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

            if (context.Transaction == null)
            {
                await connection.DisposeAsync();
            }
        }
    }
#endif

    protected internal virtual async Task OpenConnectionIfClosedAsync(DbConnection connection, CancellationToken cancellationToken)
        => await OpenConnectionIfClosedAsync(connection, Tracer, GetType().Name, cancellationToken);

    protected virtual async Task<DbTransaction> OpenTransactionAsync(DbConnection connection, IsolationLevel? isolationLevel, CancellationToken cancellationToken)
    {
        using (Tracer?.BuildSpan(GetType().Name + ".BeginTransaction").StartActive())
        {
            await OpenConnectionIfClosedAsync(connection, cancellationToken);

            return await connection.BeginTransactionAsync(isolationLevel ?? TransactionIsolationLevel, cancellationToken);
        }
    }

    private DapperCommandContext BuildDapperCommandContext(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default)
        => new DapperCommandContext()
        {
            CommandText = commandText,
            Parameters = parameters,
            Transaction = transaction,
            CommandTimeoutSeconds = commandTimeoutSeconds ?? CommandTimeoutSeconds,
            CommandType = commandType ?? CommandType.Text,
            CancellationTimeoutSeconds = cancellationTimeoutSeconds ?? CancellationTimeoutSeconds,
        };
}
