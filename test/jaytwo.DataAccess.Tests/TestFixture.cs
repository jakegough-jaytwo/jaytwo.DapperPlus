using jaytwo.DataAccess.MySql;
using jaytwo.DataAccess.Postgres;
using Microsoft.Extensions.Configuration;

namespace jaytwo.DataAccess.Tests;

public class TestFixture
{
    public TestFixture()
    {
        var assmeblyLocation = GetType().Assembly.Location;
        var basePath = new FileInfo(assmeblyLocation!).Directory!.FullName;

        TestEnvironment = Environment.GetEnvironmentVariable("TEST_ENV");

        Configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("testsettings.json")
            .AddJsonFile($"testsettings.{TestEnvironment}.json", optional: true)
            .Build();

        var postgresConnectionString = Configuration.GetConnectionString("PostgresDb")!;
        PostgresDapperWrapper = new PostgresDapperWrapper(postgresConnectionString);

        var mySqlConnectionString = Configuration.GetConnectionString("MySqlDb")!;
        MySqlDapperWrapper = new MySqlDapperWrapper(mySqlConnectionString);
    }

    public IConfiguration Configuration { get; }

    public string? TestEnvironment { get; }

    public IPostgresDapperWrapper PostgresDapperWrapper { get; }

    public IMySqlDapperWrapper MySqlDapperWrapper { get; }
}
