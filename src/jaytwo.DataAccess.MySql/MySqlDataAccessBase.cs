using MySql.Data.MySqlClient;
using OpenTracing;

namespace jaytwo.DataAccess.MySql;

public abstract class MySqlDataAccessBase : DataAccessBase<MySqlConnection, MySqlTransaction>, IMySqlDataAccess
{
    public MySqlDataAccessBase(string defaultConnectionString, ITracer? tracer = null)
        : base(defaultConnectionString, tracer)
    {
    }

    public override async Task<object> HealthCheckAsync()
    {
        if (DefaultConnectionString.StartsWith("TODO"))
        {
            return "Disabled";
        }

        var connectionStringBuilder = new MySqlConnectionStringBuilder(DefaultConnectionString);

        var result = new Dictionary<string, object>()
        {
            { "server", connectionStringBuilder.Server! },
            { "database", connectionStringBuilder.Database! },
            { "userid", connectionStringBuilder.UserID! },
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
