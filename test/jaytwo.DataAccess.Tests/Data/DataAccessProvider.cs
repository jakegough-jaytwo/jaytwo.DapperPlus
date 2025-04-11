namespace jaytwo.DataAccess.Tests.Data;

public class DataAccessProvider
{
    private readonly Func<ISampleDataAccess> _mySqlLockFactory;
    private readonly Func<ISampleDataAccess> _postgresLockFactory;

    public DataAccessProvider(
        string mySqlConnectionString,
        string postgresConnectionString)
    {
        _mySqlLockFactory = () => new SampleMySqlDataAccess(mySqlConnectionString);
        _postgresLockFactory = () => new SamplePostgresDataAccess(postgresConnectionString);
    }

    public ISampleDataAccess GetDataAccess(string moniker)
    {
        switch (moniker)
        {
            case Monikers.Postgres:
                return _postgresLockFactory.Invoke();
            case Monikers.MySql:
                return _mySqlLockFactory.Invoke();
            default:
                throw new NotSupportedException($"DataAccess type '{moniker}' is not supported.");
        }
    }
}
