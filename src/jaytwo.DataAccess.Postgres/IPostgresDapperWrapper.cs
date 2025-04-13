using Npgsql;

namespace jaytwo.DataAccess.Postgres;

public interface IPostgresDapperWrapper
    : IDapperWrapper<NpgsqlConnection, NpgsqlTransaction>
{
}
