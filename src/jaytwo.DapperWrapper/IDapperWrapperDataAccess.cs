using System.Data;
using System.Data.Common;

namespace jaytwo.DapperWrapper;

public interface IDapperWrapperDataAccess
{
    Task<object> HealthCheckAsync();

    Task RunInTransactionAsync(Func<DbTransaction, Task> callback, CancellationToken cancellationToken = default)
        => RunInTransactionAsync(callback, isolationLevel: default, cancellationToken: cancellationToken);

    Task RunInTransactionAsync(Func<DbTransaction, Task> callback, IsolationLevel? isolationLevel, CancellationToken cancellationToken = default);
}
