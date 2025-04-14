using System.Data;
using System.Data.Common;

namespace jaytwo.DapperWrapper;

public abstract class DapperWrapperDataAccess<TConnection, TTransaction>
    : DapperWrapperDataAccess
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    public DapperWrapperDataAccess(IDapperWrapper<TConnection, TTransaction> dapper)
        : base(dapper)
    {
        Dapper = dapper;
    }

    private IDapperWrapper<TConnection, TTransaction> Dapper { get; }

    public async Task RunInTransactionAsync(Func<TTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default)
        => await Dapper.RunInTransactionAsync(callback, isolationLevel, cancellationToken);

    protected virtual new TConnection CreateConnection()
        => Dapper.CreateConnection();
}
