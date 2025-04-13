using System.Data.Common;
using jaytwo.DataAccess.Tests.Data.Models;

namespace jaytwo.DataAccess.Tests.Data;

public interface ISampleDataAccess : IDapperWrapperDataAccess
{
    Task<int> InsertSampleAsync(string sampleId, double value, DateTime asOfDateUtc, DbTransaction? transaction = default, CancellationToken cancellationToken = default);

    Task<IList<SampleRow>> SelectSamplesAsync(string foo, DbTransaction? transaction = default, CancellationToken cancellationToken = default);

    Task<DateTime?> GetAsOfDateAsync(string sampleId, DbTransaction? transaction = null, CancellationToken cancellationToken = default);
}
