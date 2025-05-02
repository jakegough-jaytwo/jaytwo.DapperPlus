using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Dapper;

namespace jaytwo.DapperWrapper;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
internal class DapperCommandContext
{
    public bool HasConnection => GetContextConnection() != null;

    public string CommandText { get; set; }

    public object? Parameters { get; set; }

    public DbConnection? Connection { get; set; }

    public DbTransaction? Transaction { get; set; }

    public int CommandTimeoutSeconds { get; set; }

    public CommandType CommandType { get; set; }

    public int CancellationTimeoutSeconds { get; set; }

    public DbConnection? GetContextConnection() => Connection ?? Transaction?.Connection;

    public CommandDefinition ToDapperCommandDefinition(CommandFlags flags, CancellationToken cancellationToken)
        => new CommandDefinition(
            commandText: CommandText,
            parameters: Parameters,
            transaction: Transaction,
            commandTimeout: CommandTimeoutSeconds,
            commandType: CommandType,
            flags: flags,
            cancellationToken: cancellationToken);
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
