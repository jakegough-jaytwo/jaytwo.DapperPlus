using System.Data;
using MySql.Data.MySqlClient;
using OpenTracing;

namespace jaytwo.DapperWrapper.MySql;

public class MySqlDapperWrapper
    : DefaultDapperWrapper<MySqlConnection, MySqlTransaction>, IMySqlDapperWrapper
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
        => await new MySqDatabaseHealthCheck(this).HealthCheckAsync(cancellationToken);
}
