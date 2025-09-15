using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace jaytwo.DapperPlus.MySql;

public class MySqlDapperWrapper
    : DapperWrapper<MySqlConnection, MySqlTransaction>, IMySqlDapperWrapper
{
    public MySqlDapperWrapper(
        string connectionString,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds)
        : base(
            connectionFactory: () => new MySqlConnection(connectionString),
            transactionIsolationLevel: transactionIsolationLevel,
            commandTimeoutSeconds: commandTimeoutSeconds,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds)
    {
    }

    public MySqlDapperWrapper(
        Func<MySqlConnection> connectionFactory,
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
        => await new MySqDatabaseHealthCheck(this).HealthCheckAsync(cancellationToken);
}
