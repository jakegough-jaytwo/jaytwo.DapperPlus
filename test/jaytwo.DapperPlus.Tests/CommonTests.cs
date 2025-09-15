using System;
using System.Threading.Tasks;
using Dapper;
using jaytwo.DapperPlus.Tests.Data;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.DapperPlus.Tests;

public class CommonTests : IClassFixture<TestFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly DataAccessFactory _dataAccessProvider;

    static CommonTests()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public CommonTests(ITestOutputHelper output, TestFixture fixture)
    {
        _output = output;

        _dataAccessProvider = new DataAccessFactory(
            fixture.MySqlDapperWrapper,
            fixture.PostgresDapperWrapper,
            fixture.SqlServerDapperWrapper);
    }

    public static object[][] GetMonikerTestCases() => new object[][]
    {
        new[] { Monikers.MySql },
        new[] { Monikers.Postgres },
        new[] { Monikers.SqlServer },
    };

    [Theory]
    [MemberData(nameof(GetMonikerTestCases))]
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
    [MemberData(nameof(GetMonikerTestCases))]
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
    [MemberData(nameof(GetMonikerTestCases))]
    public async Task CanQueryUnbufferedAsync_in_transaction(string moniker)
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);
        await dataAccess.RunInTransactionAsync(
            async transaction =>
            {
                var rowsAffected = await dataAccess.InsertSampleAsync(key, value, utcNow, transaction);

                // act
                var rows = await dataAccess.SelectSamplesUnbufferedAsync(key, transaction);

                // assert
                var row = Assert.Single(rows)!;
                Assert.Equal(key, row.SampleId);
                Assert.Equal(value, row.Value!.Value, precision: 4);
                Assert.Equal(utcNow, row.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));

                await transaction.CommitAsync(); // just making sure nothing closed the connection
            });
    }

    [Theory]
    [MemberData(nameof(GetMonikerTestCases))]
    public async Task CanRunInTransactionAsync(string moniker)
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);

        await dataAccess.RunInTransactionAsync(
            async transaction =>
            {
                var rowsAffected = await dataAccess.InsertSampleAsync(key, value, utcNow, transaction);

                // act
                var rowsInsideTransaction = await dataAccess.SelectSampleAsync(key, transaction);
                await transaction.RollbackAsync(); // TODO in SqlServer, if the transaction is still open, the following select outside the transaction will hang (and i don't want to change the sql statements to be SqlServer specific)

                var rowsOutsideTransaction = await dataAccess.SelectSampleOrDefaultAsync(key);

                // assert
                Assert.Equal(key, rowsInsideTransaction.SampleId);
                Assert.Equal(value, rowsInsideTransaction.Value!.Value, precision: 4);
                Assert.Equal(utcNow, rowsInsideTransaction.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));
                Assert.Null(rowsOutsideTransaction);
            });
    }

    [Theory]
    [MemberData(nameof(GetMonikerTestCases))]
    public async Task CanQueryMultipleAsync(string moniker)
    {
        // arrange
        var key1 = Guid.NewGuid().ToString();
        var key2 = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);
        await dataAccess.InsertSampleAsync(key1, value, utcNow);
        await dataAccess.InsertSampleAsync(key2, value, utcNow);

        // act
        var multi = await dataAccess.QueryMultipleSampleRowAsync(key1, key2);

        // assert
        var row1 = Assert.Single(multi.Rows1)!;
        Assert.Equal(key1, row1.SampleId);
        Assert.Equal(value, row1.Value!.Value, precision: 4);
        Assert.Equal(utcNow, row1.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));

        var row2 = Assert.Single(multi.Rows2)!;
        Assert.Equal(key2, row2.SampleId);
        Assert.Equal(value, row2.Value!.Value, precision: 4);
        Assert.Equal(utcNow, row2.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));
    }

    [Theory]
    [MemberData(nameof(GetMonikerTestCases))]
    public async Task CanQueryMultipleAsync_with_transaction(string moniker)
    {
        // arrange
        var key1 = Guid.NewGuid().ToString();
        var key2 = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);
        await dataAccess.RunInTransactionAsync(async transaction =>
        {
            await dataAccess.InsertSampleAsync(key1, value, utcNow, transaction);
            await dataAccess.InsertSampleAsync(key2, value, utcNow, transaction);

            // act
            var multi = await dataAccess.QueryMultipleSampleRowAsync(key1, key2, transaction);

            // assert
            var row1 = Assert.Single(multi.Rows1)!;
            Assert.Equal(key1, row1.SampleId);
            Assert.Equal(value, row1.Value!.Value, precision: 4);
            Assert.Equal(utcNow, row1.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));

            var row2 = Assert.Single(multi.Rows2)!;
            Assert.Equal(key2, row2.SampleId);
            Assert.Equal(value, row2.Value!.Value, precision: 4);
            Assert.Equal(utcNow, row2.AsOfDateUtc!.Value, precision: TimeSpan.FromSeconds(1));

            await transaction.CommitAsync(); // just making sure nothing closed the connection
        });
    }

    [Theory]
    [MemberData(nameof(GetMonikerTestCases))]
    public async Task CanExecuteReaderAsync(string moniker)
    {
        // arrange
        var key1 = Guid.NewGuid().ToString();
        var key2 = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);
        await dataAccess.InsertSampleAsync(key1, value, utcNow);
        await dataAccess.InsertSampleAsync(key2, value, utcNow);

        // act
        var ids = await dataAccess.ExecuteReaderSampleIdsAsync(key1, key2);

        // assert
        Assert.Contains(key1, ids);
        Assert.Contains(key2, ids);
        Assert.Equal(2, ids.Count);
    }

    [Theory]
    [MemberData(nameof(GetMonikerTestCases))]
    public async Task CanExecuteReaderAsync_with_transaction(string moniker)
    {
        // arrange
        var key1 = Guid.NewGuid().ToString();
        var key2 = Guid.NewGuid().ToString();
        var value = 123.456;
        var utcNow = DateTime.UtcNow;

        var dataAccess = _dataAccessProvider.GetDataAccess(moniker);

        await dataAccess.RunInTransactionAsync(async transaction =>
        {
            await dataAccess.InsertSampleAsync(key1, value, utcNow, transaction);
            await dataAccess.InsertSampleAsync(key2, value, utcNow, transaction);

            // act
            var ids = await dataAccess.ExecuteReaderSampleIdsAsync(key1, key2, transaction);

            // assert
            Assert.Contains(key1, ids);
            Assert.Contains(key2, ids);
            Assert.Equal(2, ids.Count);
        });
    }

    [Theory]
    [MemberData(nameof(GetMonikerTestCases))]
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
    [MemberData(nameof(GetMonikerTestCases))]
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
    [MemberData(nameof(GetMonikerTestCases))]
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
    [MemberData(nameof(GetMonikerTestCases))]
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
