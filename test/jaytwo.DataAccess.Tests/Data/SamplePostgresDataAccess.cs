using System.Data;
using jaytwo.DataAccess.Postgres;
using jaytwo.DataAccess.Tests.Data.Models;
using OpenTracing;

namespace jaytwo.DataAccess.Tests.Data;

public class SamplePostgresDataAccess : PostgresDataAccessBase, IPostgresDataAccess, ISampleDataAccess
{
    public SamplePostgresDataAccess(string defaultConnectionString, ITracer? tracer = null)
        : base(defaultConnectionString, tracer)
    {
    }

    public override async Task<object> HealthCheckAsync()
    {
        return await base.HealthCheckAsync();
    }

    public async Task<int> InsertSample(string sampleId, double value, DateTime asOfDateUtc, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        var sql = "INSERT INTO samples (sample_id, value, as_of_date_utc) VALUES (@sample_id, @value, @as_of_date_utc)";
        var args = new
        {
            sample_id = sampleId,
            value = value,
            as_of_date_utc = asOfDateUtc,
        };
        var rowsAffected = await ExecuteAsync(sql, args, transaction, cancellationToken: cancellationToken);
        return rowsAffected;
    }

    public async Task<IList<SampleRow>> SelectSamples(string sampleId, IDbTransaction? transaction, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM samples WHERE sample_id = @sample_id";
        var args = new { sample_id = sampleId };
        var rows = await QueryAsync<SampleRow>(sql, args, transaction, cancellationToken: cancellationToken);
        return rows;
    }
}
