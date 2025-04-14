using System.Data;
using Microsoft.Data.SqlClient;
using OpenTracing;

namespace jaytwo.DapperWrapper.SqlServer;

public class SqlServerDapperWrapper
    : DefaultDapperWrapper<SqlConnection, SqlTransaction>, ISqlServerDapperWrapper
{
    public SqlServerDapperWrapper(
        string connectionString,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds,
        ITracer? tracer = default)
        : base(
            connectionFactory: () => new SqlConnection(connectionString),
            transactionIsolationLevel: transactionIsolationLevel,
            commandTimeoutSeconds: commandTimeoutSeconds,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds,
            tracer: tracer)
    {
    }

    public SqlServerDapperWrapper(
        Func<SqlConnection> connectionFactory,
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

        var connectionStringBuilder = new SqlConnectionStringBuilder(connection.ConnectionString);

        var result = new Dictionary<string, object>()
        {
            { "DataSource", connectionStringBuilder.DataSource! },
            { "InitialCatalog", connectionStringBuilder.InitialCatalog! },
            { "UserID", connectionStringBuilder.UserID! },
        };

        try
        {
            var nowTime = await ExecuteScalarAsync<DateTime>("SELECT getdate()");
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
