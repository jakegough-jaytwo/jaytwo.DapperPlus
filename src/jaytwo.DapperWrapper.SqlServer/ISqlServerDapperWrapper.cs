using Microsoft.Data.SqlClient;

namespace jaytwo.DapperWrapper.SqlServer;

public interface ISqlServerDapperWrapper
    : IDapperWrapper<SqlConnection, SqlTransaction>
{
}
