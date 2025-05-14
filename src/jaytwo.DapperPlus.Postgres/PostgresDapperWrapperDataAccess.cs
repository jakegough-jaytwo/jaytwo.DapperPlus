using Npgsql;

namespace jaytwo.DapperPlus.Postgres;

public abstract class PostgresDapperWrapperDataAccess
    : DapperWrapperDataAccess<NpgsqlConnection, NpgsqlTransaction>, IPostgresDapperWrapperDataAccess
{
    public PostgresDapperWrapperDataAccess(string connectionString)
        : base(new PostgresDapperWrapper(connectionString))
    {
    }

    public PostgresDapperWrapperDataAccess(IPostgresDapperWrapper dapperWrapper)
        : base(dapperWrapper)
    {
    }
}
