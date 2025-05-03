using Microsoft.Data.SqlClient;

namespace jaytwo.DapperPlus.SqlServer;

public interface ISqlServerDapperWrapperDataAccess
    : IDapperWrapperDataAccess<SqlConnection, SqlTransaction>
{
}
