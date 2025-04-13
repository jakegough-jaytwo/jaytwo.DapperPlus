using System.Data;
using System.Data.Common;

namespace jaytwo.DataAccess;

public interface IDapperWrapper
{
    DbConnection CreateConnection();

    Task<object> HealthCheckAsync(CancellationToken cancellationToken = default);

    Task RunInTransactionAsync(Func<DbTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken = default);

    Task<int> ExecuteAsync(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        CancellationToken cancellationToken = default);

    Task<T?> ExecuteScalarAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        CancellationToken cancellationToken = default);

    Task<IList<T>> QueryAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        T? prototype = default,
        CancellationToken cancellationToken = default);

    Task<T?> QuerySingleOrDefaultAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        T? prototype = default,
        CancellationToken cancellationToken = default);

    Task<T> QuerySingleAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        CommandType? commandType = null,
        int? cancellationTimeoutSeconds = null,
        T? prototype = default,
        CancellationToken cancellationToken = default);

#if NET5_0_OR_GREATER
    IAsyncEnumerable<T> QueryUnbufferedAsync<T>(
        string commandText,
        object? parameters = null,
        DbTransaction? transaction = null,
        int? commandTimeoutSeconds = null,
        int? cancellationTimeoutSeconds = null,
        CommandType? commandType = null,
        T? prototype = default);
#endif
}

public interface IDapperWrapper<TConnection, TTransaction> : IDapperWrapper
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    new TConnection CreateConnection();

    Task RunInTransactionAsync(
        Func<TTransaction, Task> callback,
        IsolationLevel? isolationLevel = default,
        CancellationToken cancellationToken = default);
}
