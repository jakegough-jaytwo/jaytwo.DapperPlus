using MySql.Data.MySqlClient;

namespace jaytwo.DataAccess.MySql;

public abstract class MySqlDapperWrapperDataAccess
    : DapperWrapperDataAccess<MySqlConnection, MySqlTransaction>, IMySqlDapperWrapperDataAccess
{
    public MySqlDapperWrapperDataAccess(string defaultConnectionString)
        : base(new MySqlDapperWrapper(defaultConnectionString))
    {
    }

    public MySqlDapperWrapperDataAccess(IMySqlDapperWrapper dapperWrapper)
        : base(dapperWrapper)
    {
    }
}
