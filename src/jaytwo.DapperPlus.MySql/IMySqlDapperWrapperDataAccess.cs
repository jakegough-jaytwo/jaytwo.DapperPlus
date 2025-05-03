using MySql.Data.MySqlClient;

namespace jaytwo.DapperPlus.MySql;

public interface IMySqlDapperWrapperDataAccess
    : IDapperWrapperDataAccess<MySqlConnection, MySqlTransaction>
{
}
