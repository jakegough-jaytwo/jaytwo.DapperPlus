using System.Data;
using Npgsql;
using OpenTracing;

namespace jaytwo.DataAccess.Postgres;

public class PostgresDapperWrapper
    : DapperWrapper<NpgsqlConnection, NpgsqlTransaction>, IPostgresDapperWrapper
{
    public PostgresDapperWrapper(
        string connectionString,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds,
        ITracer? tracer = default)
        : base(
            connectionFactory: () => new NpgsqlConnection(connectionString),
            transactionIsolationLevel: transactionIsolationLevel,
            commandTimeoutSeconds: commandTimeoutSeconds,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            tracer: tracer)
    {
    }

    public PostgresDapperWrapper(
        Func<NpgsqlConnection> connectionFactory,
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

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connection.ConnectionString);

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
