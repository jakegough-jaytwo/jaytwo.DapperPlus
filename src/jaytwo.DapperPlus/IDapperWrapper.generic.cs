using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.DapperPlus;

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
