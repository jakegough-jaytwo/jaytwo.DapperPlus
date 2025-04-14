using System.Data;
using System.Data.Common;

namespace jaytwo.DapperWrapper;

public interface IDapperWrapperDataAccess<TConnection, TTransaction>
    : IDapperWrapperDataAccess
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    Task RunInTransactionAsync(Func<TTransaction, Task> callback, CancellationToken cancellationToken = default)
        => RunInTransactionAsync(callback, isolationLevel: default, cancellationToken: cancellationToken);

    Task RunInTransactionAsync(Func<TTransaction, Task> callback, IsolationLevel? isolationLevel, CancellationToken cancellationToken = default);
}
