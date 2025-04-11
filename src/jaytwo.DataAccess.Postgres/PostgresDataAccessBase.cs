using Npgsql;
using OpenTracing;

namespace jaytwo.DataAccess.Postgres;

public abstract class PostgresDataAccessBase : DataAccessBase<NpgsqlConnection, NpgsqlTransaction>, IPostgresDataAccess
{
    public PostgresDataAccessBase(string defaultConnectionString, ITracer? tracer = null)
        : base(defaultConnectionString, tracer)
    {
    }

    public override async Task<object> HealthCheckAsync()
    {
        if (DefaultConnectionString.StartsWith("TODO"))
        {
            return "Disabled";
        }

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(DefaultConnectionString);

        var result = new Dictionary<string, object>()
        {
            { "host", connectionStringBuilder.Host! },
            { "database", connectionStringBuilder.Database! },
            { "username", connectionStringBuilder.Username! },
        };

        try
        {
            var nowTime = await ExecuteScalarAsync<DateTime>("SELECT now()");
            result["serverTime"] = nowTime.ToString("O");
        }
        catch (Exception ex)
        {
            var healthCheckException = new Exception(ex.Message, ex);
            healthCheckException.Data.Add(nameof(result), result);
            throw healthCheckException;
        }

        return result;
    }
}
