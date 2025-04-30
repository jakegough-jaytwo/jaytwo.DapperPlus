using MySql.Data.MySqlClient;

namespace jaytwo.DapperWrapper.MySql;

public class MySqDatabaseHealthCheck
    : DatabaseHealthCheck<MySqlDapperWrapper>
{
    public MySqDatabaseHealthCheck(MySqlDapperWrapper dapperWrapper)
        : base(dapperWrapper)
    {
    }

    protected override IDictionary<string, object> GetConnectionStringDetails(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

        return new Dictionary<string, object>()
        {
            { nameof(MySqlConnectionStringBuilder.Server), connectionStringBuilder.Server! },
            { nameof(MySqlConnectionStringBuilder.Database), connectionStringBuilder.Database! },
            { nameof(MySqlConnectionStringBuilder.UserID), connectionStringBuilder.UserID! },
        };
    }
}
