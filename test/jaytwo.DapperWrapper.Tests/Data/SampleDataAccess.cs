using System.Data;
using System.Data.Common;
using jaytwo.DapperWrapper.Tests.Data.Models;

namespace jaytwo.DapperWrapper.Tests.Data;

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
        var rowsAffected = await ExecuteAsync(sql, args, transaction, cancellationToken);
        return rowsAffected;
    }

    public async Task<IList<SampleRow>> SelectSamplesAsync(string sampleId, DbTransaction? transaction, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM samples WHERE sample_id = @sample_id";
        var args = new { sample_id = sampleId };
        var result = await QueryAsync<SampleRow>(sql, args, transaction, cancellationToken);
        return result;
    }

    public async Task<IList<SampleRow>> SelectSamplesUnbufferedAsync(string sampleId, DbTransaction? transaction, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM samples WHERE sample_id = @sample_id";
        var args = new { sample_id = sampleId };
        var result = QueryUnbufferedAsync<SampleRow>(sql, args, transaction);
        return await result.ToListAsync(cancellationToken);
    }

    public async Task<(IList<SampleRow> Rows1, IList<SampleRow> Rows2)> QueryMultipleSampleRowAsync(string sampleId1, string sampleId2, DbTransaction? transaction, CancellationToken cancellationToken)
    {
        var sql = @"
SELECT * FROM samples WHERE sample_id = @sample_id1;
SELECT * FROM samples WHERE sample_id = @sample_id2;
";

        var args = new { sample_id1 = sampleId1, sample_id2 = sampleId2 };
        using var gridReadeer = await QueryMultipleAsync(sql, args, transaction, cancellationToken);
        {
            var rows1 = (await gridReadeer.ReadAsync<SampleRow>()).ToList();
            var rows2 = (await gridReadeer.ReadAsync<SampleRow>()).ToList();
            return (Rows1: rows1, Rows2: rows2);
        }
    }

    public async Task<IList<string>> ExecuteReaderSampleIdsAsync(string sampleId1, string sampleId2, DbTransaction? transaction = default, CommandBehavior? commandBehavior = default, CancellationToken cancellationToken = default)
    {
        var result = new List<string>();

        var sql = @"
SELECT * FROM samples WHERE sample_id = @sample_id1;
SELECT * FROM samples WHERE sample_id = @sample_id2;
";

        var args = new { sample_id1 = sampleId1, sample_id2 = sampleId2 };

        using (var reader = await ExecuteReaderAsync(sql, args, transaction, commandBehavior: commandBehavior, cancellationToken: cancellationToken))
        {
            do
            {
                while (reader.Read())
                {
                    var item = reader.GetString(reader.GetOrdinal("sample_id"));
                    result.Add(item);
                }
            }
            while (reader.NextResult());
        }

        return result;
    }

    public async Task<SampleRow> SelectSampleAsync(string sampleId, DbTransaction? transaction, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM samples WHERE sample_id = @sample_id";
        var args = new { sample_id = sampleId };
        var result = await QuerySingleAsync<SampleRow>(sql, args, transaction, cancellationToken);
        return result;
    }

    public async Task<SampleRow?> SelectSampleOrDefaultAsync(string sampleId, DbTransaction? transaction, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM samples WHERE sample_id = @sample_id";
        var args = new { sample_id = sampleId };
        var result = await QuerySingleOrDefaultAsync<SampleRow>(sql, args, transaction, cancellationToken);
        return result;
    }

    public async Task<DateTime?> GetAsOfDateAsync(string sampleId, DbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        var sql = "SELECT as_of_date_utc FROM samples WHERE sample_id = @sample_id";
        var args = new { sample_id = sampleId };
        var result = await ExecuteScalarAsync<DateTime?>(sql, args, transaction, cancellationToken);
        return result;
    }
}
