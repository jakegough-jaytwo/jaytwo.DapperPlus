using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Dapper;
using OpenTelemetry.Trace;
using static Dapper.SqlMapper;

namespace jaytwo.DapperPlus;

public class DapperWrapper
    : IDapperWrapper
{
    public const IsolationLevel DefaultTransactionIsolationLevel = IsolationLevel.Unspecified;
    public const int DefaultCommandTimeoutSeconds = 30;
    public const int DefaultCancellationTimeoutSeconds = 45;

    private readonly Func<DbConnection> _connectionFactory;

    public DapperWrapper(
        Func<DbConnection> connectionFactory,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds)
    {
        _connectionFactory = connectionFactory;
        TransactionIsolationLevel = transactionIsolationLevel ?? DefaultTransactionIsolationLevel;
        CommandTimeoutSeconds = commandTimeoutSeconds ?? DefaultCommandTimeoutSeconds;
        CancellationTimeoutSeconds = cancellationTimeoutSeconds ?? DefaultCancellationTimeoutSeconds;
    }

    // this constructor overload is purely so Moq can find the single-argument constructor
    internal DapperWrapper(Func<DbConnection> connectionFactory)
        : this(connectionFactory, default, default, default)
    {
    }

    public static string ActivitySourceName { get; } = "jaytwo.DapperPlus.DapperWrapper";

    public IsolationLevel TransactionIsolationLevel { get; }

    public int CancellationTimeoutSeconds { get; }

    public int CommandTimeoutSeconds { get; }

    private static ActivitySource ActivitySource { get; } = new(ActivitySourceName);

    public virtual async Task<object> HealthCheckAsync(CancellationToken cancellationToken = default)
        => await new DatabaseHealthCheck<DapperWrapper>(this).HealthCheckAsync(cancellationToken);

    public virtual DbConnection CreateConnection()
        => _connectionFactory.Invoke();

    public virtual async Task RunInTransactionAsync(Func<DbTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default)
    {
        using (var connection = CreateConnection())
        using (var transaction = await BeginTransactionAsync(connection, isolationLevel, cancellationToken))
        {
            await callback.Invoke(transaction);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    public virtual async Task<DbTransaction> BeginTransactionAsync(DbConnection connection, IsolationLevel? isolationLevel, CancellationToken cancellationToken)
    {
        using (var activity = ActivitySource.StartActivity(GetType().Name + "." + nameof(BeginTransactionAsync), ActivityKind.Client))
        {
            await OpenConnectionIfClosedAsync(connection, cancellationToken);

            return await connection.BeginTransactionAsync(isolationLevel ?? TransactionIsolationLevel, cancellationToken);
        }
    }

    public virtual async Task CommitTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default)
    {
        using (var activity = ActivitySource.StartActivity(GetType().Name + "." + nameof(CommitTransactionAsync), ActivityKind.Client))
        {
            await transaction.CommitAsync(cancellationToken);
        }
    }

    public virtual async Task RollbackTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default)
    {
        using (var activity = ActivitySource.StartActivity(GetType().Name + "." + nameof(RollbackTransactionAsync), ActivityKind.Client))
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

    public virtual async Task<IDataReader> ExecuteReaderAsync(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        CommandBehavior? commandBehavior = default,
        CancellationToken cancellationToken = default)
        => await ExecuteReaderAsync(
            BuildDapperCommandContext(
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeoutSeconds: commandTimeoutSeconds,
                commandType: commandType,
                cancellationTimeoutSeconds: cancellationTimeoutSeconds),
            commandBehavior ?? CommandBehavior.Default,
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

    public virtual async Task<IGridReaderWrapper> QueryMultipleAsync(
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

    internal static async Task OpenConnectionIfClosedAsync(DbConnection connection, string? activityNamePrefix = default, CancellationToken cancellationToken = default)
    {
        if (connection.State == ConnectionState.Closed)
        {
            await OpenConnectionAsync(connection, activityNamePrefix, cancellationToken);
        }
    }

    internal static async Task OpenConnectionAsync(DbConnection connection, string? activityNamePrefix = default, CancellationToken cancellationToken = default)
    {
        using (var activity = ActivitySource.StartActivity(activityNamePrefix + nameof(OpenConnectionAsync), ActivityKind.Client))
        {
            await connection.OpenAsync(cancellationToken);
        }
    }

    internal virtual async Task<int> ExecuteAsync(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.ExecuteAsync(comm),
            context,
            CommandFlags.Buffered,
            disposeConnection: !context.HasConnection,
            cancellationToken);

    internal virtual async Task<T?> ExecuteScalarAsync<T>(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.ExecuteScalarAsync<T>(comm),
            context,
            CommandFlags.Buffered,
            disposeConnection: !context.HasConnection,
            cancellationToken);

    internal virtual async Task<IDataReader> ExecuteReaderAsync(DapperCommandContext context, CommandBehavior commandBehavior, CancellationToken cancellationToken)
    {
        if (!context.HasConnection)
        {
            commandBehavior |= CommandBehavior.CloseConnection;
        }

        return await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.ExecuteReaderAsync(comm, commandBehavior),
            context,
            CommandFlags.None,
            disposeConnection: false,
            cancellationToken);
    }

    internal virtual async Task<IList<T>> QueryAsync<T>(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => (await conn.QueryAsync<T>(comm)).ToList(),
            context,
            CommandFlags.Buffered,
            disposeConnection: !context.HasConnection,
            cancellationToken);

    internal virtual async Task<T?> QuerySingleOrDefaultAsync<T>(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.QuerySingleOrDefaultAsync<T>(comm),
            context,
            CommandFlags.Buffered,
            disposeConnection: !context.HasConnection,
            cancellationToken);

    internal virtual async Task<T> QuerySingleAsync<T>(DapperCommandContext context, CancellationToken cancellationToken)
        => await RunWithCancellationTokenAsync(
            async (conn, comm) => await conn.QuerySingleAsync<T>(comm),
            context,
            CommandFlags.Buffered,
            disposeConnection: !context.HasConnection,
            cancellationToken);

    internal virtual async Task<IGridReaderWrapper> QueryMultipleAsync(DapperCommandContext context, CancellationToken cancellationToken)
    {
        return await RunWithCancellationTokenAsync(
            async (conn, comm) =>
            {
                var gridReader = await conn.QueryMultipleAsync(comm);
                return new GridReaderWrapper(conn, gridReader, disposeConnection: !context.HasConnection);
            },
            context,
            CommandFlags.Buffered,
            disposeConnection: false,
            cancellationToken);
    }

    internal virtual async Task<T> RunWithCancellationTokenAsync<T>(
        Func<DbConnection, CommandDefinition, Task<T>> queryDelegate,
        DapperCommandContext context,
        CommandFlags commandFlags,
        bool disposeConnection,
        CancellationToken cancellationToken)
    {
        var activityName = GetType().Name + "." + nameof(RunWithCancellationTokenAsync);
        using var activity = ActivitySource.StartActivity(activityName, ActivityKind.Client);
        activity?.SetTag("db.commandText", context.CommandText);

        var connection = context.GetContextConnection() ?? _connectionFactory();

        // just making sure to open the connection explicitly so we have the telemetry if the operation requires opening the connection
        await OpenConnectionIfClosedAsync(connection, activityNamePrefix: activityName, cancellationToken);

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
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                activity?.RecordException(ex);
                throw;
            }
            finally
            {
                if (disposeConnection)
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
        var activityName = GetType().Name + "." + nameof(QueryUnbufferedAsync);
        using var activity = ActivitySource.StartActivity(activityName, ActivityKind.Client);
        activity?.SetTag("db.commandText", context.CommandText);

        var connection = context.GetContextConnection() ?? _connectionFactory();

        // just making sure to open the connection explicitly so we have the telemetry if the operation requires opening the connection
        await OpenConnectionIfClosedAsync(connection, activityNamePrefix: activityName, cancellationToken);

        using var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(context.CancellationTimeoutSeconds));
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token);
        var firstRowActivity = ActivitySource.StartActivity(activityName + ".FirstRow", ActivityKind.Client);
        activity?.SetTag("firstRow.span.name", firstRowActivity?.DisplayName);

        try
        {
            var rows = queryDelegate(connection).WithCancellation(linkedTokenSource.Token);

            var enumerator = rows.GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync())
            {
                firstRowActivity?.Dispose();

                yield return enumerator.Current;

                while (await enumerator.MoveNextAsync())
                {
                    yield return enumerator.Current;
                }
            }
        }
        finally
        {
            // TODO: we can't do a catch with a yield, figure out if there's some other way to wrap a try/catch so we can do activity?.SetStatus(ActivityStatusCode.Error, ex.Message)

            firstRowActivity?.Dispose();

            if (context.Transaction == null)
            {
                await connection.DisposeAsync();
            }
        }
    }
#endif

    protected internal virtual async Task OpenConnectionIfClosedAsync(DbConnection connection, CancellationToken cancellationToken)
        => await OpenConnectionIfClosedAsync(connection, GetType().Name, cancellationToken);

    private DapperCommandContext BuildDapperCommandContext(
        string commandText,
        object? parameters = default,
        DbConnection? connection = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default)
        => new DapperCommandContext()
        {
            CommandText = commandText,
            Parameters = parameters,
            Connection = connection,
            Transaction = transaction,
            CommandTimeoutSeconds = commandTimeoutSeconds ?? CommandTimeoutSeconds,
            CommandType = commandType ?? CommandType.Text,
            CancellationTimeoutSeconds = cancellationTimeoutSeconds ?? CancellationTimeoutSeconds,
        };
}
