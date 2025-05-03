using Npgsql;

namespace jaytwo.DapperPlus.Postgres;

public interface IPostgresDapperWrapperDataAccess
    : IDapperWrapperDataAccess<NpgsqlConnection, NpgsqlTransaction>
{
}
