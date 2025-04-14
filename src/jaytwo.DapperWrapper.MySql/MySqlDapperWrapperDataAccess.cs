using MySql.Data.MySqlClient;

namespace jaytwo.DapperWrapper.MySql;

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
