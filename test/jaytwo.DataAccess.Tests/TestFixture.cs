using System;
using System.IO;
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

        PostgresConnectionString = Configuration.GetConnectionString("PostgresDb")!;

        MySqlConnectionString = Configuration.GetConnectionString("MySqlDb")!;
    }

    public IConfiguration Configuration { get; }

    public string? TestEnvironment { get; }

    public string PostgresConnectionString { get; }

    public string MySqlConnectionString { get; }
}
