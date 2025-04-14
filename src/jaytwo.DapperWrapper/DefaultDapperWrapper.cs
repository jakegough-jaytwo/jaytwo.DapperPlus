using System.Data;
using System.Data.Common;
using System.Diagnostics;
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

    private readonly InternalDapper _dapper;
    private readonly Func<DbConnection> _connectionFactory;

    public DefaultDapperWrapper(
        Func<DbConnection> connectionFactory,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds,
        ITracer? tracer = default)
        : this(
              connectionFactory,
              new InternalDapper(
                transactionIsolationLevel ?? DefaultTransactionIsolationLevel,
                commandTimeoutSeconds ?? DefaultCommandTimeoutSeconds,
                cancellationTimeoutSeconds ?? DefaultCancellationTimeoutSeconds,
                tracer),
              tracer)
    {
    }

    internal DefaultDapperWrapper(Func<DbConnection> connectionFactory, InternalDapper dapper, ITracer? tracer)
    {
        _connectionFactory = connectionFactory;
        _dapper = dapper;
        Tracer = tracer;
    }

    public IsolationLevel TransactionIsolationLevel => _dapper.TransactionIsolationLevel;

    public int CancellationTimeoutSeconds => _dapper.CancellationTimeoutSeconds;

    public int CommandTimeoutSeconds => _dapper.CommandTimeoutSeconds;

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
        => await _dapper.RunWithCancellationTokenAsync(
            connectionFactory: () => CreateConnection(),
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
        => await _dapper.RunWithCancellationTokenAsync<T?>(
            connectionFactory: () => CreateConnection(),
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
        => await _dapper.RunWithCancellationTokenAsync<IList<T>>(
            connectionFactory: () => CreateConnection(),
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
        => await _dapper.RunWithCancellationTokenAsync<GridReader>(
            connectionFactory: () => CreateConnection(),
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
        => await _dapper.RunWithCancellationTokenAsync<T?>(
            connectionFactory: () => CreateConnection(),
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
        => await _dapper.RunWithCancellationTokenAsync<T>(
            connectionFactory: () => CreateConnection(),
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
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        T? prototype = default)
        => _dapper.QueryUnbufferedAsync<T>(
            connectionFactory: () => CreateConnection(),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds);
#endif

    protected virtual async Task OpenConnectionIfClosedAsync(DbConnection connection, CancellationToken cancellationToken)
        => await InternalDapper.OpenConnectionIfClosedAsync(connection, Tracer, GetType().Name + ".", cancellationToken);

    protected virtual async Task<DbTransaction> OpenTransactionAsync(DbConnection connection, IsolationLevel? isolationLevel, CancellationToken cancellationToken)
    {
        using (Tracer?.BuildSpan(GetType().Name + ".BeginTransaction").StartActive())
        {
            await OpenConnectionIfClosedAsync(connection, cancellationToken);

            return await connection.BeginTransactionAsync(isolationLevel ?? TransactionIsolationLevel, cancellationToken);
        }
    }
}
