using jaytwo.DataAccess.Postgres;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.DataAccess.Tests;

public class PostgresTests : IClassFixture<TestFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly IPostgresDapperWrapper _postgres;

    public PostgresTests(TestFixture fixture, ITestOutputHelper output)
    {
        _postgres = fixture.PostgresDapperWrapper;
        _output = output;
    }

    [Fact]
    public void ConnectionStringHasDetails()
    {
        using var connection = _postgres.CreateConnection();
        var connectionString = connection.ConnectionString;
        _output.WriteLine("Connection Sring: " + _postgres.CreateConnection().ConnectionString);

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString!);

        Assert.NotNull(connectionStringBuilder.Host);
        Assert.NotNull(connectionStringBuilder.Database);
        Assert.NotNull(connectionStringBuilder.Username);
        Assert.NotNull(connectionStringBuilder.Password);
    }

    [Fact]
    public async Task CanConnect()
    {
        using var connection = _postgres.CreateConnection();

        await connection.OpenAsync();

        Assert.Equal(System.Data.ConnectionState.Open, connection.State);
    }

    [Fact]
    public async Task HealthCheckAsync()
    {
        // Arrange

        // Act
        var healthCheck = await _postgres.HealthCheckAsync();

        // Assert
        Assert.NotNull(healthCheck);

        var healthCheckJson = System.Text.Json.JsonSerializer.Serialize(healthCheck);
        _output.WriteLine("Health Check: " + healthCheckJson);
    }
}
