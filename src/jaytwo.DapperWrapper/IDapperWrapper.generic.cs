using System.Data;
using System.Data.Common;

namespace jaytwo.DapperWrapper;

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
