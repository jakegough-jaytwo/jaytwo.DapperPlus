using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Dapper;
using static Dapper.SqlMapper;

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

    public virtual async Task RunInTransactionAsync(Func<DbTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default)
        => await Dapper.RunInTransactionAsync(callback, isolationLevel, cancellationToken);

    protected virtual DbConnection CreateConnection()
        => Dapper.CreateConnection();

    protected virtual async Task CommitTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default)
        => await Dapper.CommitTransactionAsync(transaction, cancellationToken);

    protected virtual async Task RollbackTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default)
        => await Dapper.RollbackTransactionAsync(transaction, cancellationToken);

    protected async Task<int> ExecuteAsync(string commandText, CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            commandText: commandText,
            parameters: default,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<int> ExecuteAsync(
        string commandText,
        object? parameters,
        CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            commandText: commandText,
            parameters: parameters,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<int> ExecuteAsync(
        string commandText,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            commandText: commandText,
            parameters: default,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<int> ExecuteAsync(
        string commandText,
        object? parameters,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected virtual async Task<int> ExecuteAsync(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        CancellationToken cancellationToken = default)
        => await Dapper.ExecuteAsync(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

    protected async Task<T?> ExecuteScalarAsync<T>(string commandText, CancellationToken cancellationToken = default)
        => await ExecuteScalarAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<T?> ExecuteScalarAsync<T>(
        string commandText,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await ExecuteScalarAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<T?> ExecuteScalarAsync<T>(
        string commandText,
        object? parameters,
        CancellationToken cancellationToken = default)
        => await ExecuteScalarAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<T?> ExecuteScalarAsync<T>(
        string commandText,
        object? parameters,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await ExecuteScalarAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<T?> ExecuteScalarAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        CancellationToken cancellationToken = default)
        => await Dapper.ExecuteScalarAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

    protected async Task<IList<T>> QueryAsync<T>(string commandText, CancellationToken cancellationToken = default)
        => await QueryAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<IList<T>> QueryAsync<T>(
        string commandText,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await QueryAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<IList<T>> QueryAsync<T>(
        string commandText,
        object? parameters,
        CancellationToken cancellationToken = default)
        => await QueryAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<IList<T>> QueryAsync<T>(
        string commandText,
        object? parameters,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await QueryAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<IList<T>> QueryAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
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

    protected async Task<T?> QuerySingleOrDefaultAsync<T>(string commandText, CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<T?> QuerySingleOrDefaultAsync<T>(
        string commandText,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<T?> QuerySingleOrDefaultAsync<T>(
        string commandText,
        object? parameters,
        CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<T?> QuerySingleOrDefaultAsync<T>(
        string commandText,
        object? parameters,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<T?> QuerySingleOrDefaultAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
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

    protected async Task<T> QuerySingleAsync<T>(string commandText, CancellationToken cancellationToken = default)
        => await QuerySingleAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<T> QuerySingleAsync<T>(
        string commandText,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await QuerySingleAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<T> QuerySingleAsync<T>(
        string commandText,
        object? parameters,
        CancellationToken cancellationToken = default)
        => await QuerySingleAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<T> QuerySingleAsync<T>(
        string commandText,
        object? parameters,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await QuerySingleAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default,
            cancellationToken: cancellationToken);

    protected async Task<T> QuerySingleAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
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

    protected async Task<GridReader> QueryMultipleAsync(string commandText, CancellationToken cancellationToken = default)
        => await QueryMultipleAsync(
            commandText: commandText,
            parameters: default,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<GridReader> QueryMultipleAsync(
        string commandText,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await QueryMultipleAsync(
            commandText: commandText,
            parameters: default,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<GridReader> QueryMultipleAsync(
        string commandText,
        object? parameters,
        CancellationToken cancellationToken = default)
        => await QueryMultipleAsync(
            commandText: commandText,
            parameters: parameters,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<GridReader> QueryMultipleAsync(
        string commandText,
        object? parameters,
        DbTransaction? transaction,
        CancellationToken cancellationToken = default)
        => await QueryMultipleAsync(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            cancellationToken: cancellationToken);

    protected async Task<GridReader> QueryMultipleAsync(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        CommandType? commandType = default,
        int? cancellationTimeoutSeconds = default,
        CancellationToken cancellationToken = default)
        => await Dapper.QueryMultipleAsync(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: commandTimeoutSeconds,
            commandType: commandType,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            cancellationToken: cancellationToken);

#if NET5_0_OR_GREATER

    protected IAsyncEnumerable<T> QueryUnbufferedAsync<T>(string commandText)
        => QueryUnbufferedAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default);

    protected IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        string commandText,
        DbTransaction? transaction)
        => QueryUnbufferedAsync<T>(
            commandText: commandText,
            parameters: default,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default);

    protected IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        string commandText,
        object? parameters)
        => QueryUnbufferedAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: default,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default);

    protected IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        string commandText,
        object? parameters,
        DbTransaction? transaction)
        => QueryUnbufferedAsync<T>(
            commandText: commandText,
            parameters: parameters,
            transaction: transaction,
            commandTimeoutSeconds: default,
            commandType: default,
            cancellationTimeoutSeconds: default,
            prototype: default);

    protected virtual IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        string commandText,
        object? parameters = default,
        DbTransaction? transaction = default,
        int? commandTimeoutSeconds = default,
        int? cancellationTimeoutSeconds = default,
        CommandType? commandType = default,
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

public abstract class DapperWrapperDataAccess<TConnection, TTransaction>
    : DapperWrapperDataAccess
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    public DapperWrapperDataAccess(IDapperWrapper<TConnection, TTransaction> dapper)
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
