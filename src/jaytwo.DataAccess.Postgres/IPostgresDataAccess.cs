using Npgsql;

namespace jaytwo.DataAccess.Postgres;

public interface IPostgresDataAccess : IDataAccess<NpgsqlConnection, NpgsqlTransaction>
{
}
