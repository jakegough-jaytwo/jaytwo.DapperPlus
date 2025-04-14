using Microsoft.Data.SqlClient;

namespace jaytwo.DapperWrapper.SqlServer;

public abstract class SqlServerDapperWrapperDataAccess
    : DapperWrapperDataAccess<SqlConnection, SqlTransaction>, ISqlServerDapperWrapperDataAccess
{
    public SqlServerDapperWrapperDataAccess(string defaultConnectionString)
        : base(new SqlServerDapperWrapper(defaultConnectionString))
    {
    }

    public SqlServerDapperWrapperDataAccess(ISqlServerDapperWrapper dapperWrapper)
        : base(dapperWrapper)
    {
    }
}
