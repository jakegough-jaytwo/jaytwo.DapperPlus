using MySql.Data.MySqlClient;

namespace jaytwo.DataAccess.MySql;

public interface IMySqlDataAccess : IDataAccess<MySqlConnection, MySqlTransaction>
{
}
