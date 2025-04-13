using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Dapper;

namespace jaytwo.DataAccess;

#pragma warning disable SA1402 // File may only contain a single type
public abstract class DapperWrapperDataAccess
    : IDapperWrapperDataAccess
{
    static DapperWrapperDataAccess()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    protected DapperWrapperDataAccess(IDapperWrapper dapper)
    {
        Dapper = dapper;
    }

    private IDapperWrapper Dapper { get; }

    public virtual async Task<object> HealthCheckAsync()
        => await Dapper.HealthCheckAsync();

    public virtual async Task RunInTransactionAsync(Func<DbTransaction, Task> callback, IsolationLevel? isolationLevel = null, CancellationToken cancellationToken = default)
        => await Dapper.RunInTransactionAsync(callback, isolationLevel, cancellationToken);

    protected virtual DbConnection CreateConnection()
        => Dapper.CreateConnection();

    protected virtual async Task CommitTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default)
        => await Dapper.CommitTransactionAsync(transaction, cancellationToken);

    protected virtual async Task RollbackTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default)
        => await Dapper.RollbackTransactionAsync(transaction, cancellationToken);

    protected virtual async Task<int> ExecuteAsync(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        CancellationToken cancellationToken = default)
        => await Dapper.ExecuteAsync(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

    protected async Task<T?> ExecuteScalarAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        CancellationToken cancellationToken = default)
        => await Dapper.ExecuteScalarAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

    protected async Task<IList<T>> QueryAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        T? prototype = default,
        CancellationToken cancellationToken = default)
        => await Dapper.QueryAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            prototype: prototype,
            cancellationToken: cancellationToken);

    protected async Task<T?> QuerySingleOrDefaultAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        T? prototype = default,
        CancellationToken cancellationToken = default)
        => await Dapper.QuerySingleOrDefaultAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            prototype: prototype,
            cancellationToken: cancellationToken);

    protected async Task<T> QuerySingleAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        T? prototype = default,
        CancellationToken cancellationToken = default)
        => await Dapper.QuerySingleAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            prototype: prototype,
            cancellationToken: cancellationToken);

#if NET5_0_OR_GREATER
    protected virtual IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        int? cancellationTimeoutSeconds = null,
        CommandType? commandType = null,
        T? prototype = default)
        => Dapper.QueryUnbufferedAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            prototype: prototype);
#endif
}

public abstract class DataAccessBase<TConnection, TTransaction>
    : DapperWrapperDataAccess
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    public DataAccessBase(IDapperWrapper<TConnection, TTransaction> dapper)
        : base(dapper)
    {
        Dapper = dapper;
    }

    private IDapperWrapper<TConnection, TTransaction> Dapper { get; }

    public async Task RunInTransactionAsync(Func<TTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default)
        => await Dapper.RunInTransactionAsync(callback, isolationLevel, cancellationToken);

    protected virtual new TConnection CreateConnection()
        => Dapper.CreateConnection();
}
#pragma warning restore SA1402 // File may only contain a single type
