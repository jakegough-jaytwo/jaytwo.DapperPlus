using jaytwo.DataAccess.MySql;
using MySql.Data.MySqlClient;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.DataAccess.Tests;

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
    public void ConnectionStringHasDetails()
    {
        using var connection = _mySql.CreateConnection();
        var connectionString = connection.ConnectionString;
        _output.WriteLine("Connection Sring: " + connectionString);

        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
        Assert.NotNull(connectionStringBuilder.Server);
        Assert.NotNull(connectionStringBuilder.Database);
        Assert.NotNull(connectionStringBuilder.UserID);
        Assert.NotNull(connectionStringBuilder.Password);
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
