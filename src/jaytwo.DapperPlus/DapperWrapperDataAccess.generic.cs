using System.Data;
using System.Data.Common;

namespace jaytwo.DapperPlus;

public abstract class DapperWrapperDataAccess<TConnection, TTransaction>
    : DapperWrapperDataAccess
    where TConnection : DbConnection
    where TTransaction : DbTransaction
{
    public DapperWrapperDataAccess(IDapperWrapper<TConnection, TTransaction> dapper)
        : base(dapper)
    {
    }

    protected new IDapperWrapper<TConnection, TTransaction> Dapper
        => (IDapperWrapper<TConnection, TTransaction>)base.Dapper;

    public async Task RunInTransactionAsync(Func<TTransaction, Task> callback, IsolationLevel? isolationLevel = default, CancellationToken cancellationToken = default)
        => await Dapper.RunInTransactionAsync(callback, isolationLevel, cancellationToken);

    protected virtual new TConnection CreateConnection()
        => Dapper.CreateConnection();
}
