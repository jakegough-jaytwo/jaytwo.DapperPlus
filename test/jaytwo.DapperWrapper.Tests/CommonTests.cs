using jaytwo.DapperWrapper.Tests.Data;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.DapperWrapper.Tests;

public class CommonTests : IClassFixture<TestFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly DataAccessFactory _dataAccessProvider;

    public CommonTests(ITestOutputHelper output, TestFixture fixture)
    {
        _output = output;

        _dataAccessProvider = new DataAccessFactory(
            fixture.MySqlDapperWrapper,
            fixture.PostgresDapperWrapper);
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
        var rowsAffected = await dataAccess.InsertSampleAsync(key, 1, DateTime.UtcNow);

        // assert
        Assert.NotEqual(0, rowsAffected);
    }

    [Theory]
    [InlineData(Monikers.MySql)]
    [InlineData(Monikers.Postgres)]
    public async Task CanQueryUnbufferedAsync(string moniker)
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);
        var rowsAffected = await dataAccess.InsertSampleAsync(key, value, utcNow);

        // act
        var rows = await dataAccess.SelectSamplesUnbufferedAsync(key);

        // assert
        var row = Assert.Single(rows)!;
        Assert.Equal(key, row.SampleId);
        Assert.Equal(value, row.Value!.Value, precision: 4);
        Assert.Equal(utcNow, row.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(Monikers.MySql)]
    [InlineData(Monikers.Postgres)]
    public async Task CanRunInTransactionAsync(string moniker)
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);

        await dataAccess.RunInTransactionAsync(async transaction =>
        {
            var rowsAffected = await dataAccess.InsertSampleAsync(key, value, utcNow, transaction);

            // act
            var rowsInsideTransaction = await dataAccess.SelectSampleAsync(key, transaction);
            var rowsOutsideTransaction = await dataAccess.SelectSampleOrDefaultAsync(key);

            // assert
            Assert.Equal(key, rowsInsideTransaction.SampleId);
            Assert.Equal(value, rowsInsideTransaction.Value!.Value, precision: 4);
            Assert.Equal(utcNow, rowsInsideTransaction.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));
            Assert.Null(rowsOutsideTransaction);
        });
    }

    // TODO: tests for querymultiple

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
        var rowsAffected = await dataAccess.InsertSampleAsync(key, value, utcNow);

        // act
        var rows = await dataAccess.SelectSamplesAsync(key);

        // assert
        var row = Assert.Single(rows)!;
        Assert.Equal(key, row.SampleId);
        Assert.Equal(value, row.Value!.Value, precision: 4);
        Assert.Equal(utcNow, row.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(Monikers.MySql)]
    [InlineData(Monikers.Postgres)]
    public async Task CanQuerySingleAsync(string moniker)
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;
        var missingKey = Guid.NewGuid().ToString();

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);
        var rowsAffected = await dataAccess.InsertSampleAsync(key, value, utcNow);

        // act
        var row = await dataAccess.SelectSampleAsync(key);
        var thrownException = await Assert.ThrowsAsync<InvalidOperationException>(() => dataAccess.SelectSampleAsync(missingKey));

        // assert
        Assert.Equal(key, row.SampleId);
        Assert.Equal(value, row.Value!.Value, precision: 4);
        Assert.Equal(utcNow, row.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(Monikers.MySql)]
    [InlineData(Monikers.Postgres)]
    public async Task CanQuerySingleOrDefaultAsync(string moniker)
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;
        var missingKey = Guid.NewGuid().ToString();

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);
        var rowsAffected = await dataAccess.InsertSampleAsync(key, value, utcNow);

        // act
        var foundRow = await dataAccess.SelectSampleAsync(key);
        var missingRow = await dataAccess.SelectSampleOrDefaultAsync(missingKey);

        // assert
        Assert.Equal(key, foundRow.SampleId);
        Assert.Equal(value, foundRow.Value!.Value, precision: 4);
        Assert.Equal(utcNow, foundRow.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));
        Assert.Null(missingRow);
    }

    [Theory]
    [InlineData(Monikers.MySql)]
    [InlineData(Monikers.Postgres)]
    public async Task CanExecuteScalarAsync(string moniker)
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);
        var rowsAffected = await dataAccess.InsertSampleAsync(key, value, utcNow);

        // act
        var result = await dataAccess.GetAsOfDateAsync(key);

        // assert
        Assert.NotNull(result);
        Assert.Equal(utcNow, result!.Value, precision: TimeSpan.FromSeconds(1));
    }
}
