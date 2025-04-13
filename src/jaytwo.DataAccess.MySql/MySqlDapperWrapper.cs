using System.Data;
using MySql.Data.MySqlClient;
using OpenTracing;

namespace jaytwo.DataAccess.MySql;

public class MySqlDapperWrapper
    : DapperWrapper<MySqlConnection, MySqlTransaction>, IMySqlDapperWrapper
{
    public MySqlDapperWrapper(
        string connectionString,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds,
        ITracer? tracer = default)
        : base(
            connectionFactory: () => new MySqlConnection(connectionString),
            transactionIsolationLevel: transactionIsolationLevel,
            commandTimeoutSeconds: commandTimeoutSeconds,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            tracer: tracer)
    {
    }

    public MySqlDapperWrapper(
        Func<MySqlConnection> connectionFactory,
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

    public override async Task<object> HealthCheckAsync(CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();

        if (connection.ConnectionString.StartsWith("TODO"))
        {
            return "Disabled";
        }

        var connectionStringBuilder = new MySqlConnectionStringBuilder(connection.ConnectionString);

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
