using MySql.Data.MySqlClient;

namespace jaytwo.DapperWrapper.MySql;

public interface IMySqlDapperWrapperDataAccess
    : IDapperWrapperDataAccess<MySqlConnection, MySqlTransaction>
{
}
