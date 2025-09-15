using System.Threading.Tasks;
using jaytwo.DapperPlus.SqlServer;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.DapperPlus.Tests;

public class SqlServerTests : IClassFixture<TestFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly ISqlServerDapperWrapper _sqlServer;

    public SqlServerTests(TestFixture fixture, ITestOutputHelper output)
    {
        _sqlServer = fixture.SqlServerDapperWrapper;
        _output = output;
    }

    [Fact]
    public async Task CanConnect()
    {
        using var connection = _sqlServer.CreateConnection();

        await connection.OpenAsync();

        Assert.Equal(System.Data.ConnectionState.Open, connection.State);
    }

    [Fact]
    public async Task HealthCheckAsync()
    {
        // Arrange

        // Act
        var healthCheck = await _sqlServer.HealthCheckAsync();

        // Assert
        Assert.NotNull(healthCheck);

        var healthCheckJson = System.Text.Json.JsonSerializer.Serialize(healthCheck);
        _output.WriteLine("Health Check: " + healthCheckJson);
    }
}
