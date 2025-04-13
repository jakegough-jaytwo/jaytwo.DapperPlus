using System.Data;
using System.Data.Common;

namespace jaytwo.DataAccess;

public interface IDapperWrapperDataAccess
{
    Task<object> HealthCheckAsync();

    Task RunInTransactionAsync(Func<DbTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default);
}

public interface IDapperWrapperDataAccess<TConnection, TTransaction>
    : IDapperWrapperDataAccess
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    Task RunInTransactionAsync(Func<TTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default);
}
