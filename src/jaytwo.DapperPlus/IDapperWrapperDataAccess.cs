using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.DapperPlus;

public interface IDapperWrapperDataAccess
{
    Task<object> HealthCheckAsync(CancellationToken cancellationToken = default);

    async Task RunInTransactionAsync(Func<DbTransaction, Task> callback, CancellationToken cancellationToken = default)
        => await RunInTransactionAsync(callback, isolationLevel: default, cancellationToken: cancellationToken);

    Task RunInTransactionAsync(Func<DbTransaction, Task> callback, IsolationLevel? isolationLevel, CancellationToken cancellationToken = default);
}
