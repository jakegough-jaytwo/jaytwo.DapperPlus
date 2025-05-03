using System.Data;
using Microsoft.Data.SqlClient;
using OpenTracing;

namespace jaytwo.DapperPlus.SqlServer;

public class SqlServerDapperWrapper
    : DapperWrapper<SqlConnection, SqlTransaction>, ISqlServerDapperWrapper
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
        => await new SqlServerDatabaseHealthCheck(this).HealthCheckAsync(cancellationToken);
}
