using System.Threading.Tasks;
using jaytwo.DapperPlus.MySql;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.DapperPlus.Tests;

public class MySqlTests : IClassFixture<TestFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly IMySqlDapperWrapper _mySql;

    public MySqlTests(TestFixture fixture, ITestOutputHelper output)
    {
        _mySql = fixture.MySqlDapperWrapper;
        _output = output;
    }

    [Fact]
    public async Task CanConnect()
    {
        using var connection = _mySql.CreateConnection();

        await connection.OpenAsync();

        Assert.Equal(System.Data.ConnectionState.Open, connection.State);
    }

    [Fact]
    public async Task HealthCheckAsync()
    {
        // Arrange

        // Act
        var healthCheck = await _mySql.HealthCheckAsync();

        // Assert
        Assert.NotNull(healthCheck);

        var healthCheckJson = System.Text.Json.JsonSerializer.Serialize(healthCheck);
        _output.WriteLine("Health Check: " + healthCheckJson);
    }
}
