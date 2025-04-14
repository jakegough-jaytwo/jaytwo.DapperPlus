using MySql.Data.MySqlClient;

namespace jaytwo.DapperWrapper.MySql;

public interface IMySqlDapperWrapper
    : IDapperWrapper<MySqlConnection, MySqlTransaction>
{
}
