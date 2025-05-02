using System.Data;
using System.Data.Common;
using OpenTracing;

namespace jaytwo.DapperWrapper;

public class DefaultDapperWrapper<TConnection, TTransaction>
    : DefaultDapperWrapper, IDapperWrapper<TConnection, TTransaction>
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    public DefaultDapperWrapper(
        Func<TConnection> connectionFactory,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds,
        ITracer? tracer = default)
        : base(
            connectionFactory: connectionFactory,
            transactionIsolationLevel: transactionIsolationLevel,
            commandTimeoutSeconds: commandTimeoutSeconds,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            tracer: tracer)
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
