using Microsoft.Data.SqlClient;

namespace jaytwo.DapperPlus.SqlServer;

public interface ISqlServerDapperWrapper
    : IDapperWrapper<SqlConnection, SqlTransaction>
{
}
