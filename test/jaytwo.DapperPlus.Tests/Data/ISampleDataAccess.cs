using System.Data;
using System.Data.Common;
using jaytwo.DapperPlus.Tests.Data.Models;
using static Dapper.SqlMapper;

namespace jaytwo.DapperPlus.Tests.Data;

public interface ISampleDataAccess : IDapperWrapperDataAccess
{
    Task<int> InsertSampleAsync(string sampleId, double value, DateTime asOfDateUtc, DbTransaction? transaction = default, CancellationToken cancellationToken = default);

    Task<IList<SampleRow>> SelectSamplesAsync(string sampleId, DbTransaction? transaction = default, CancellationToken cancellationToken = default);

    Task<IList<SampleRow>> SelectSamplesUnbufferedAsync(string sampleId, DbTransaction? transaction = default, CancellationToken cancellationToken = default);

    Task<(IList<SampleRow> Rows1, IList<SampleRow> Rows2)> QueryMultipleSampleRowAsync(string sampleId1, string sampleId2, DbTransaction? transaction = default, CancellationToken cancellationToken = default);

    Task<IList<string>> ExecuteReaderSampleIdsAsync(string sampleId1, string sampleId2, DbTransaction? transaction = default, CommandBehavior? commandBehavior = default, CancellationToken cancellationToken = default);

    Task<SampleRow> SelectSampleAsync(string sampleId, DbTransaction? transaction = default, CancellationToken cancellationToken = default);

    Task<SampleRow?> SelectSampleOrDefaultAsync(string sampleId, DbTransaction? transaction = default, CancellationToken cancellationToken = default);

    Task<DateTime?> GetAsOfDateAsync(string sampleId, DbTransaction? transaction = null, CancellationToken cancellationToken = default);
}
