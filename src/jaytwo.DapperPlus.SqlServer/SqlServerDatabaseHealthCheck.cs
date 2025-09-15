using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace jaytwo.DapperPlus.SqlServer;

public class SqlServerDatabaseHealthCheck
    : DatabaseHealthCheck<SqlServerDapperWrapper>
{
    public SqlServerDatabaseHealthCheck(SqlServerDapperWrapper dapperWrapper)
        : base(dapperWrapper)
    {
    }

    protected override IDictionary<string, object> GetConnectionStringDetails(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

        return new Dictionary<string, object>()
        {
            { nameof(SqlConnectionStringBuilder.DataSource), connectionStringBuilder.DataSource! },
            { nameof(SqlConnectionStringBuilder.InitialCatalog), connectionStringBuilder.InitialCatalog! },
            { nameof(SqlConnectionStringBuilder.UserID), connectionStringBuilder.UserID! },
        };
    }
}
