using System.Data;
using jaytwo.DataAccess.Tests.Data.Models;

namespace jaytwo.DataAccess.Tests.Data;

public interface ISampleDataAccess
{
    Task<int> InsertSample(string sampleId, double value, DateTime asOfDateUtc, IDbTransaction? transaction = default, CancellationToken cancellationToken = default);

    Task<IList<SampleRow>> SelectSamples(string foo, IDbTransaction? transaction = default, CancellationToken cancellationToken = default);
}
