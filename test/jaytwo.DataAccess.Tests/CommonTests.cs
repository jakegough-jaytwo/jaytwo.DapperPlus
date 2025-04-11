using jaytwo.DataAccess.Tests.Data;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.DataAccess.Tests;

public class CommonTests : IClassFixture<TestFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly DataAccessProvider _dataAccessProvider;

    public CommonTests(ITestOutputHelper output, TestFixture fixture)
    {
        _output = output;

        _dataAccessProvider = new DataAccessProvider(
            fixture.MySqlConnectionString,
            fixture.PostgresConnectionString);
    }

    [Theory]
    [InlineData(Monikers.MySql)]
    [InlineData(Monikers.Postgres)]
    public async Task CanExecuteAsync(string moniker)
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);

        // act
        var rowsAffected = await dataAccess.InsertSample(key, 1, DateTime.UtcNow);

        // assert
        Assert.NotEqual(0, rowsAffected);
    }

    [Theory]
    [InlineData(Monikers.MySql)]
    [InlineData(Monikers.Postgres)]
    public async Task CanQueryAsync(string moniker)
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);
        var rowsAffected = await dataAccess.InsertSample(key, value, utcNow);

        // act
        var rows = await dataAccess.SelectSamples(key);

        // assert
        var row = Assert.Single(rows)!;
        Assert.Equal(key, row.SampleId);
        Assert.Equal(value, row.Value!.Value, precision: 4);
        Assert.Equal(utcNow, row.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));
    }
}
