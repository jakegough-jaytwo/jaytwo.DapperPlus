using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace jaytwo.DapperPlus.Postgres;

public class PostgresDapperWrapper
    : DapperWrapper<NpgsqlConnection, NpgsqlTransaction>, IPostgresDapperWrapper
{
    public PostgresDapperWrapper(
        string connectionString,
        IsolationLevel? transactionIsolationLevel = DefaultTransactionIsolationLevel,
        int? commandTimeoutSeconds = DefaultCommandTimeoutSeconds,
        int? cancellationTimeoutSeconds = DefaultCancellationTimeoutSeconds)
        : base(
            connectionFactory: () => new NpgsqlConnection(connectionString),
            transactionIsolationLevel: transactionIsolationLevel,
            commandTimeoutSeconds: commandTimeoutSeconds,
            cancellationTimeoutSeconds: cancellationTimeoutSeconds)
    {
    }

    public PostgresDapperWrapper(
        Func<NpgsqlConnection> connectionFactory,
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
        => await new PostgresDatabaseHealthCheck(this).HealthCheckAsync(cancellationToken);
}
