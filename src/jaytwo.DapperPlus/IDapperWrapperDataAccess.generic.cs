using System.Data;
using System.Data.Common;

namespace jaytwo.DapperPlus;

public interface IDapperWrapperDataAccess<TConnection, TTransaction>
    : IDapperWrapperDataAccess
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    async Task RunInTransactionAsync(Func<TTransaction, Task> callback, CancellationToken cancellationToken = default)
        => await RunInTransactionAsync(callback, isolationLevel: default, cancellationToken: cancellationToken);

    Task RunInTransactionAsync(Func<TTransaction, Task> callback, IsolationLevel? isolationLevel, CancellationToken cancellationToken = default);
}
