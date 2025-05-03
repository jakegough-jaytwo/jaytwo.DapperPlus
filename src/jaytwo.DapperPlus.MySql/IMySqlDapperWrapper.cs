using MySql.Data.MySqlClient;

namespace jaytwo.DapperPlus.MySql;

public interface IMySqlDapperWrapper
    : IDapperWrapper<MySqlConnection, MySqlTransaction>
{
}
