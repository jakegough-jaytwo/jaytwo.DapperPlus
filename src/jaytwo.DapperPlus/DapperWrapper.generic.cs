using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.DapperPlus;

public class DapperWrapper<TConnection, TTransaction>
    : DapperWrapper, IDapperWrapper<TConnection, TTransaction>
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    public DapperWrapper(
        Func<TConnection> connectionFactory,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds)
        : base(
            connectionFactory: connectionFactory,
            transactionIsolationLevel: transactionIsolationLevel,
            commandTimeoutSeconds: commandTimeoutSeconds,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds)
    {
    }

    public new TConnection CreateConnection()
        => (TConnection)base.CreateConnection();

    public async Task RunInTransactionAsync(Func<TTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default)
    {
        using (var connection = CreateConnection())
        using (var transaction = await BeginTransactionAsync(connection, isolationLevel, cancellationToken))
        {
            await callback.Invoke(transaction);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    protected async Task<TTransaction> BeginTransactionAsync(TConnection connection, IsolationLevel? isolationLevel, CancellationToken cancellationToken)
        => (TTransaction)await base.BeginTransactionAsync(connection, isolationLevel, cancellationToken);
}
