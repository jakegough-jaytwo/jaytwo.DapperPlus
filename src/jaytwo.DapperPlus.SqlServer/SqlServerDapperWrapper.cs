using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace jaytwo.DapperPlus.SqlServer;

public class SqlServerDapperWrapper
    : DapperWrapper<SqlConnection, SqlTransaction>, ISqlServerDapperWrapper
{
    public SqlServerDapperWrapper(
        string connectionString,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds)
        : base(
            connectionFactory: () => new SqlConnection(connectionString),
            transactionIsolationLevel: transactionIsolationLevel,
            commandTimeoutSeconds: commandTimeoutSeconds,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds)
    {
    }

    public SqlServerDapperWrapper(
        Func<SqlConnection> connectionFactory,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds)
        : base(
            connectionFactory: connectionFactory,
            transactionIsolationLevel: transactionIsolationLevel,
            commandTimeoutSeconds: commandTimeoutSeconds,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds)
    {
    }

    public override async Task<object> HealthCheckAsync(CancellationToken cancellationToken = default)
        => await new SqlServerDatabaseHealthCheck(this).HealthCheckAsync(cancellationToken);
}
