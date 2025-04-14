using Npgsql;

namespace jaytwo.DapperWrapper.Postgres;

public interface IPostgresDapperWrapper
    : IDapperWrapper<NpgsqlConnection, NpgsqlTransaction>
{
}
