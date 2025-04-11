using System.Data;
using System.Data.Common;
using Dapper;
using OpenTracing;

namespace jaytwo.DataAccess;

public abstract class DataAccessBase<TConnection, TTransaction>
        where TConnection : DbConnection
        where TTransaction : DbTransaction
{
    static DataAccessBase()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public DataAccessBase(string defaultConnectionString, ITracer? tracer = null)
    {
        Tracer = tracer;
        DefaultConnectionString = defaultConnectionString;
    }

    public virtual IsolationLevel DefaultIsolationLevel { get; } = IsolationLevel.Unspecified;

    public virtual TimeSpan DefaultCancellationTimeout { get; } = TimeSpan.FromSeconds(45);

    public virtual TimeSpan DefaultCommandTimeout { get; } = TimeSpan.FromSeconds(30);

    protected string DefaultConnectionString { get; }

    protected ITracer? Tracer { get; }

    public abstract Task<object> HealthCheckAsync();

    public async Task RunInTransactionAsync(Func<TTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default)
        => await RunInTransactionAsync(
            connectionString: null,
            callback: callback,
            isolationLevel: isolationLevel,
            cancellationToken: cancellationToken);

    public async Task RunInTransactionAsync(string? connectionString, Func<TTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default)
    {
        using (var connection = CreateConnection(connectionString))
        using (var transaction = await OpenTransactionAsync(connection, isolationLevel, cancellationToken))
        {
            await callback.Invoke(transaction);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    public async Task CommitTransactionAsync(TTransaction transaction, CancellationToken cancellationToken = default)
    {
        using (var openConnectionScope = Tracer?.BuildSpan(GetType().Name + ".CommitTransaction").StartActive())
        {
            await transaction.CommitAsync(cancellationToken);
        }
    }

    protected async Task<TTransaction> OpenTransactionAsync(TConnection connection, IsolationLevel? isolationLevel, CancellationToken cancellationToken)
    {
        using (var openConnectionScope = Tracer?.BuildSpan(GetType().Name + ".OpenConnection").StartActive())
        {
            await connection.OpenAsync(cancellationToken);
        }

        using (var openTransactionScope = Tracer?.BuildSpan(GetType().Name + ".BeginTransaction").StartActive())
        {
            return (TTransaction)await connection.BeginTransactionAsync(isolationLevel ?? DefaultIsolationLevel, cancellationToken);
        }
    }

    protected async Task<int> ExecuteAsync(
        string commandText,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CommandFlags flags = CommandFlags.Buffered,
        TimeSpan? cancellationTimeout = null,
        CancellationToken cancellationToken = default)
        => await RunWithCancellationTokenAsync(
             queryDelegate: (conn, comm) => conn.ExecuteAsync(comm),
             commandText: commandText,
             parameters: parameters,
             transaction: transaction,
             commandTimeout: commandTimeout,
             commandType: commandType,
             flags: flags,
             cancellationTimeout: cancellationTimeout,
             cancellationToken: cancellationToken);

    protected async Task<IEnumerable<T>> QueryEnumerableAsync<T>(
        string commandText,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CommandFlags flags = CommandFlags.Buffered,
        TimeSpan? cancellationTimeout = null,
        CancellationToken cancellationToken = default,
        T prototype = default!)
        => await RunWithCancellationTokenAsync<IEnumerable<T>>(
            queryDelegate: async (conn, comm) => await conn.QueryAsync<T>(comm),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeout: commandTimeout,
            commandType: commandType,
            flags: flags,
            cancellationTimeout: cancellationTimeout,
            cancellationToken: cancellationToken);

    protected async Task<IList<T>> QueryAsync<T>(
        string commandText,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CommandFlags flags = CommandFlags.Buffered,
        TimeSpan? cancellationTimeout = null,
        CancellationToken cancellationToken = default,
        T prototype = default!)
        => await RunWithCancellationTokenAsync<IList<T>>(
            queryDelegate: async (conn, comm) => (await conn.QueryAsync<T>(comm)).ToList(),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeout: commandTimeout,
            commandType: commandType,
            flags: flags,
            cancellationTimeout: cancellationTimeout,
            cancellationToken: cancellationToken);

    protected async Task<T?> QuerySingleOrDefaultAsync<T>(
        string commandText,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CommandFlags flags = CommandFlags.Buffered,
        TimeSpan? cancellationTimeout = null,
        CancellationToken cancellationToken = default,
        T prototype = default!)
        => await RunWithCancellationTokenAsync<T?>(
            queryDelegate: (conn, comm) => conn.QuerySingleOrDefaultAsync<T>(comm),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeout: commandTimeout,
            commandType: commandType,
            flags: flags,
            cancellationTimeout: cancellationTimeout,
            cancellationToken: cancellationToken);

    protected async Task<T> QuerySingleAsync<T>(
        string commandText,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CommandFlags flags = CommandFlags.Buffered,
        TimeSpan? cancellationTimeout = null,
        CancellationToken cancellationToken = default,
        T prototype = default!)
        => await RunWithCancellationTokenAsync<T>(
            queryDelegate: (conn, comm) => conn.QuerySingleAsync<T>(comm),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeout: commandTimeout,
            commandType: commandType,
            flags: flags,
            cancellationTimeout: cancellationTimeout,
            cancellationToken: cancellationToken);

    protected async Task<T?> ExecuteScalarAsync<T>(
        string commandText,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CommandFlags flags = CommandFlags.Buffered,
        TimeSpan? cancellationTimeout = null,
        CancellationToken cancellationToken = default)
        => await RunWithCancellationTokenAsync<T?>(
            queryDelegate: (conn, comm) => conn.ExecuteScalarAsync<T?>(comm),
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeout: commandTimeout,
            commandType: commandType,
            flags: flags,
            cancellationTimeout: cancellationTimeout,
            cancellationToken: cancellationToken);

    protected virtual TConnection CreateConnection(string? connectionString = null)
        => (TConnection)Activator.CreateInstance(typeof(TConnection), connectionString ?? DefaultConnectionString)!;

    private async Task<T> RunWithCancellationTokenAsync<T>(
        Func<IDbConnection, CommandDefinition, Task<T>> queryDelegate,
        string commandText,
        object? parameters,
        IDbTransaction? transaction,
        int? commandTimeout,
        CommandType? commandType,
        CommandFlags flags,
        TimeSpan? cancellationTimeout,
        CancellationToken cancellationToken)
    {
        using var scope = Tracer?.BuildSpan(GetType().Name + "." + nameof(RunWithCancellationTokenAsync)).StartActive();

        using (var timeoutCancellationTokenSource = new CancellationTokenSource(cancellationTimeout ?? DefaultCancellationTimeout))
        using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token))
        {
            var commandDefinition = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                commandTimeout: commandTimeout,
                commandType: commandType,
                flags: flags,
                cancellationToken: linkedTokenSource.Token);

            try
            {
                if (transaction == null)
                {
                    using (var connection = CreateConnection())
                    {
                        return await queryDelegate.Invoke(connection, commandDefinition);
                    }
                }
                else
                {
                    return await queryDelegate.Invoke(transaction.Connection!, commandDefinition);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }
    }
}
