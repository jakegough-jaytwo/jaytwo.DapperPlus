using Microsoft.Data.SqlClient;

namespace jaytwo.DapperPlus.SqlServer;

public abstract class SqlServerDapperWrapperDataAccess
    : DapperWrapperDataAccess<SqlConnection, SqlTransaction>, ISqlServerDapperWrapperDataAccess
{
    public SqlServerDapperWrapperDataAccess(string connectionString)
        : base(new SqlServerDapperWrapper(connectionString))
    {
    }

    public SqlServerDapperWrapperDataAccess(ISqlServerDapperWrapper dapperWrapper)
        : base(dapperWrapper)
    {
    }
}
