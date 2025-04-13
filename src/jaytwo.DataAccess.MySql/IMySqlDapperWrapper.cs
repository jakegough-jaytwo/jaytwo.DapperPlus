using MySql.Data.MySqlClient;

namespace jaytwo.DataAccess.MySql;

public interface IMySqlDapperWrapper
    : IDapperWrapper<MySqlConnection, MySqlTransaction>
{
}
