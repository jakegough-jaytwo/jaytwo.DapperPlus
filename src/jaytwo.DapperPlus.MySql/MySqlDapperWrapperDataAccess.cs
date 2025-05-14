using MySql.Data.MySqlClient;

namespace jaytwo.DapperPlus.MySql;

public abstract class MySqlDapperWrapperDataAccess
    : DapperWrapperDataAccess<MySqlConnection, MySqlTransaction>, IMySqlDapperWrapperDataAccess
{
    public MySqlDapperWrapperDataAccess(string connectionString)
        : base(new MySqlDapperWrapper(connectionString))
    {
    }

    public MySqlDapperWrapperDataAccess(IMySqlDapperWrapper dapperWrapper)
        : base(dapperWrapper)
    {
    }
}
