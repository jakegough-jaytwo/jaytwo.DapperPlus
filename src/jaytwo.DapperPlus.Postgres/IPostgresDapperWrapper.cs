using Npgsql;

namespace jaytwo.DapperPlus.Postgres;

public interface IPostgresDapperWrapper
    : IDapperWrapper<NpgsqlConnection, NpgsqlTransaction>
{
}
