using Npgsql;

namespace jaytwo.DapperPlus.Postgres;

public class PostgresDatabaseHealthCheck
    : DatabaseHealthCheck<PostgresDapperWrapper>
{
    public PostgresDatabaseHealthCheck(PostgresDapperWrapper dapperWrapper)
        : base(dapperWrapper)
    {
    }

    protected override IDictionary<string, object> GetConnectionStringDetails(string connectionString)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);

        return new Dictionary<string, object>
        {
            { nameof(NpgsqlConnectionStringBuilder.Host), connectionStringBuilder.Host! },
            { nameof(NpgsqlConnectionStringBuilder.Database), connectionStringBuilder.Database! },
            { nameof(NpgsqlConnectionStringBuilder.Username), connectionStringBuilder.Username! },
        };
    }
}
