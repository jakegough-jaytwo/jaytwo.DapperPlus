using System.Data.Common;
using jaytwo.DataAccess.Tests.Data.Models;

namespace jaytwo.DataAccess.Tests.Data;

public class SampleDataAccess
    : DapperWrapperDataAccess, ISampleDataAccess
{
    public SampleDataAccess(IDapperWrapper dapper)
        : base(dapper)
    {
    }

    public async Task<int> InsertSampleAsync(string sampleId, double value, DateTime asOfDateUtc, DbTransaction? transaction = null, CancellationToken cancellationToken = default)
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

    public async Task<IList<SampleRow>> SelectSamplesAsync(string sampleId, DbTransaction? transaction, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM samples WHERE sample_id = @sample_id";
        var args = new { sample_id = sampleId };
        var rows = await QueryAsync<SampleRow>(sql, args, transaction, cancellationToken: cancellationToken);
        return rows;
    }

    public async Task<DateTime?> GetAsOfDateAsync(string sampleId, DbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        var sql = "SELECT as_of_date_utc FROM samples WHERE sample_id = @sample_id";
        var args = new { sample_id = sampleId };
        var result = await ExecuteScalarAsync<DateTime?>(sql, args, transaction, cancellationToken: cancellationToken);
        return result;
    }
}
