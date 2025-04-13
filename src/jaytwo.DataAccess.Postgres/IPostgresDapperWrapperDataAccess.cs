using Npgsql;

namespace jaytwo.DataAccess.Postgres;

public interface IPostgresDapperWrapperDataAccess
    : IDapperWrapperDataAccess<NpgsqlConnection, NpgsqlTransaction>
{
}
