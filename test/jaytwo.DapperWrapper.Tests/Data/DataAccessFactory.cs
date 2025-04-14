using jaytwo.DapperWrapper.MySql;
using jaytwo.DapperWrapper.Postgres;

namespace jaytwo.DapperWrapper.Tests.Data;

public class DataAccessFactory
{
    private readonly Dictionary<string, Func<ISampleDataAccess>> _dataAccessFactories = new();

    public DataAccessFactory(
        IMySqlDapperWrapper mySqlDapperWrapper,
        IPostgresDapperWrapper postgresDapperWrapper)
        : this(
            (Monikers.MySql, () => new SampleDataAccess(mySqlDapperWrapper)),
            (Monikers.Postgres, () => new SampleDataAccess(postgresDapperWrapper)))
    {
    }

    public DataAccessFactory(params (string Moniker, Func<ISampleDataAccess> Callback)[] factories)
    {
        foreach (var factory in factories)
        {
            _dataAccessFactories.Add(factory.Moniker, factory.Callback);
        }
    }

    public ISampleDataAccess GetDataAccess(string moniker)
        => _dataAccessFactories.TryGetValue(moniker, out var factory)
            ? factory.Invoke()
            : throw new NotSupportedException($"DataAccess type '{moniker}' is not supported.");
}
