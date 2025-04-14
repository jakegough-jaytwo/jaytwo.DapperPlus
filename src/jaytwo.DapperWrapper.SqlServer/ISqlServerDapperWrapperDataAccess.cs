using Microsoft.Data.SqlClient;

namespace jaytwo.DapperWrapper.SqlServer;

public interface ISqlServerDapperWrapperDataAccess
    : IDapperWrapperDataAccess<SqlConnection, SqlTransaction>
{
}
