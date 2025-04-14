using Npgsql;

namespace jaytwo.DapperWrapper.Postgres;

public interface IPostgresDapperWrapperDataAccess
    : IDapperWrapperDataAccess<NpgsqlConnection, NpgsqlTransaction>
{
}
