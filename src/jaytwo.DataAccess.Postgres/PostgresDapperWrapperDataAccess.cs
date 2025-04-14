using Npgsql;

namespace jaytwo.DataAccess.Postgres;

public abstract class PostgresDapperWrapperDataAccess
    : DapperWrapperDataAccess<NpgsqlConnection, NpgsqlTransaction>, IPostgresDapperWrapperDataAccess
{
    public PostgresDapperWrapperDataAccess(string defaultConnectionString)
        : base(new PostgresDapperWrapper(defaultConnectionString))
    {
    }

    public PostgresDapperWrapperDataAccess(IPostgresDapperWrapper dapperWrapper)
        : base(dapperWrapper)
    {
    }
}
