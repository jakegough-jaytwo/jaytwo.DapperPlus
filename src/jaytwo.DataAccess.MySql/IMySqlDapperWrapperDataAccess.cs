using MySql.Data.MySqlClient;

namespace jaytwo.DataAccess.MySql;

public interface IMySqlDapperWrapperDataAccess
    : IDapperWrapperDataAccess<MySqlConnection, MySqlTransaction>
{
}
