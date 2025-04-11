using System.Data;
using System.Data.Common;

namespace jaytwo.DataAccess;

public interface IDataAccess<TConnection, TTransaction>
        where TConnection : DbConnection
        where TTransaction : DbTransaction
{
    public abstract Task<object> HealthCheckAsync();

    Task RunInTransactionAsync(Func<TTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(TTransaction transaction, CancellationToken cancellationToken = default);
}
