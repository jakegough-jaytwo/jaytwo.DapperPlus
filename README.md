# jaytwo.DapperPlus

| Package |   |   |
|---------|---|---|
| `jaytwo.DapperPlus` | [![NuGet Version](https://img.shields.io/nuget/v/jaytwo.DapperPlus.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/jaytwo.DapperPlus) | [![NuGet Downloads](https://img.shields.io/nuget/dt/jaytwo.DapperPlus.svg?style=flat)](https://www.nuget.org/packages/jaytwo.DapperPlus) |
| `jaytwo.DapperPlus.Postgres` | [![NuGet Version](https://img.shields.io/nuget/v/jaytwo.DapperPlus.Postgres.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/jaytwo.DapperPlus.Postgres) | [![NuGet Downloads](https://img.shields.io/nuget/dt/jaytwo.DapperPlus.Postgres.svg?style=flat)](https://www.nuget.org/packages/jaytwo.DapperPlus.Postgres) |
| `jaytwo.DapperPlus.MySql` | [![NuGet Version](https://img.shields.io/nuget/v/jaytwo.DapperPlus.MySql.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/jaytwo.DapperPlus.MySql) | [![NuGet Downloads](https://img.shields.io/nuget/dt/jaytwo.DapperPlus.MySql.svg?style=flat)](https://www.nuget.org/packages/jaytwo.DapperPlus.MySql) |
| `jaytwo.DapperPlus.SqlServer` | [![NuGet Version](https://img.shields.io/nuget/v/jaytwo.DapperPlus.SqlServer.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/jaytwo.DapperPlus.SqlServer) | [![NuGet Downloads](https://img.shields.io/nuget/dt/jaytwo.DapperPlus.SqlServer.svg?style=flat)](https://www.nuget.org/packages/jaytwo.DapperPlus.SqlServer) |


**jaytwo.DapperPlus** is an opinionated extension library for [Dapper](https://github.com/DapperLib/Dapper) that introduces data access base classes, cancellation token overloads, standardized command execution patterns, and OpenTelemetry support.

[![View on GitHub](https://img.shields.io/badge/View%20on-GitHub-181717?logo=github)](https://github.com/jakegough-jaytwo/jaytwo.DapperPlus)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://mit-license.org/)

## Features

* **Provider-Specific Extensions**: Tailored support for PostgreSQL, MySQL, and SQL Server through dedicated packages.
* **Async-First API**: Embrace asynchronous programming patterns for scalable applications.
* **Cancellation Token Support**: Seamlessly propagate `CancellationToken` to Dapper operations, ensuring responsive and cancellable database interactions.
* **CancellationTimeout Support**: Specify a CancellationTimeout to abort the operation after a set duration—covering connection open, transaction begin, execution, and result enumeration—even when `CommandTimeout` alone wouldn’t apply.
* **OpenTelemetry Support**: Built-in support for OpenTelemetry, enabling easy instrumentation and monitoring of database operations.
* **DataAccess Base Classes**: Less boilerplate with data access patterns with base classes that manage connection lifetimes, transactions, and command execution.

## Installation

Add the NuGet package for your flavor of SQL:

```powershell
PM> Install-Package jaytwo.DapperPlus.MySql
PM> Install-Package jaytwo.DapperPlus.Postgres
PM> Install-Package jaytwo.DapperPlus.SqlServer
```

If you have another flavor of SQL you can always just use the common package:

```powershell
PM> Install-Package jaytwo.DapperPlus
```

## Usage

### Basic DataAccess

```csharp
public class SampleDataAccess : MySqlDapperWrapperDataAccess
{
    public SampleDataAccess(string connectionString)
        : base(connectionString)
    {
    }

    public async Task<IList<SampleRow>> SelectSamplesAsync(string sampleId, CancellationToken cancellationToken)
    {
        var sql = "SELECT * FROM samples WHERE sample_id = @sample_id";
        var args = new { sample_id = sampleId };
        var result = await QueryAsync<SampleRow>(sql, args, cancellationToken);
        return result;
    }
}
```

> You might notice: "This looks exactly like Dapper, except I'm calling the `QueryAsync()` instance method on my DataAccess class instead of using `connection.QueryAsync()`."  Exactly—that's the point.

### Coordinating Inserts with Transactions

```csharp
public class SampleDataAccess : MySqlDapperWrapperDataAccess
{
    public SampleDataAccess(string connectionString)
        : base(connectionString)
    {
    }

    public async Task InsertMeasurementSessionAndValuesAsync(string sessionId, double[] values, CancellationToken cancellationToken)
    {
        await RunInTransactionAsync(
            async transaction =>
            {
                await InsertMeasurementSessionAsync(sessionId, transaction, cancellationToken);

                foreach (var value in values)
                {
                    await InsertMeasurementValueAsync(sessionId, value, transaction, cancellationToken);
                }

                await _dataAccess.CommitTransaction(transaction, cancellationToken);
            });
    }

    private async Task InsertMeasurementSessionAsync(string sessionId, DbTransaction? transaction, CancellationToken cancellationToken)
    {
        var sql = "INSERT INTO measurement_sessions (session_id) VALUES (@session_id)";
        var args = new { session_id = sessionId };
        await ExecuteAsync(sql, args, transaction, cancellationToken);
    }

    public async Task InsertMeasurementValueAsync(string sessionId, double value, DbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        var sql = "INSERT INTO measurement_values (session_id, value) VALUES (@session_id, @value)";
        var args = new { session_id = sessionId, value };
        await ExecuteAsync(sql, args, transaction, cancellationToken);
    }
}
```

> The `RunInTransactionAsync` helper automatically opens the connection, begins the transaction, and rolls it back on failure—so your service logic stays clean and focused.

### Wrapping External Dependencies in a Transaction-Safe Way

Imagine you have to upload something to S3 and then insert a row into the database.  You want to make sure that if the S3 upload fails, the database transaction is rolled back.

Some people don't like transactions leaking into their logic layer services.  For me, it's acceptable as long as the service class doesn't have to manage connections or transaction lifetimes.

Since I hate the magic of `TransactionScope`, this is how I handle such a use case:

```csharp
public class SampleService
{
    private readonly SampleDataAccess _dataAccess;
    private readonly S3Client _s3;

    public SampleService(SampleDataAccess dataAccess, S3Client s3)
    {
        _dataAccess = dataAccess;
        _s3 = s3;
    }

    public async Task InsertSampleSamplesAsync(string sampleId, byte[] imageData, CancellationToken cancellationToken)
    {
        var s3Key = $"samples/{sampleId}.jpg";

        await _dataAccess.RunInTransactionAsync(
            async transaction =>
            {
                await _dataAccess.InsertSampleAsync(sampleId, transaction, cancellationToken);
                await _s3.UploadAsync(s3Key, imageData, cancellationToken);
                await _dataAccess.CommitTransactionAsync(transaction, cancellationToken);
            });
    }
}
```

## Background

### TL;DR

Dapper offers exceptional performance and simplicity.  Dapper works nearly always for nearly everyone.

But long-running queries (including large result sets) can highlight some of Dapper's weaknesses.  It's not that Dapper doesn't support things like cancellation tokens or end-to-end operation timeouts... it just requires a lot of boilerplate.

That boilerplate is what led me to create this package.

### The Longer Story

Dapper is already great—I've been a huge fan for years. But I ran into trouble on a REST API project where database performance issues led to multiple HTTP request retries after their HTTP client would time out. The real problem? The webserver and database host were both still waiting for the original request to finish—the database connection stayed open and the heavy query kept running. From the client’s perspective, the old request was done and dusted, but on the backend, the load just kept compounding with every retry.

I figured this would be easy to solve: add a `CancellationToken` parameter to the controller methods (which is automatically tied to client disconnection), and propagate that token down to the DB calls.

...or so I thought.

I quickly ran into a few problems:

**Problem 1: Obscure overloads**
The Dapper extension method overloads to pass in the `CancellationToken` are relatively obscure, requiring you to build a `CommandDefinition` object. Not the end of the world, but it added just that much more noise in the code.

**Problem 2: End-to-end timeouts**
Client disconnects may have been what highlighted the problem, but we wanted to proactively abort long-running queries too.  The `CommandTimeout` is a good start, but what if the command is somewhere in the middle of returning millions of rows? Or what if it's still trying to open the connection?  Since we're all `async` all the way down, it's a perfect use for a `CancellationTokenSource`.  But again, more boilerplate and more noise.

**Problem 3: Instrumentation**
I wanted to measure timings for connection open, command execution, and result enumeration. Whether using `Stopwatch` or OpenTelemetry—it’s a lot of boilerplate without structure.

I know this may go against Dapper’s minimalist spirit. Dapper is meant to be a lightweight, stateless micro-ORM for people who want to write SQL without full ADO boilerplate. This project adds connection factories and base classes—getting a little close to that "EF context" smell.

But here’s the thing: I **like** writing clean, explicit SQL. I want full control over DTOs, transactions, and connection lifetimes. I don’t want the magic of `TransactionScope` or EF’s automatic change tracking.

Most of the time, yeah, I just want a list of results. But sometimes? I want to crack open a good old-fashioned `DataReader`. This wrapper gives me that flexibility—without having to manually build `DbCommand` objects and manage connection lifetimes myself.

## When to Use

Use `jaytwo.DapperPlus` when:

- You want full control over SQL, DTOs, and connection lifetimes
- You need reliable cancellation behavior across async flows
- You're exporting large datasets or handling long-running queries
- You want OpenTelemetry traces for DB calls without boilerplate
- You’re tired of writing repetitive ADO.NET boilerplate just to get cancellation, timeouts, and tracing right

---

Made with &hearts; by Jake — Licensed under the [MIT License](https://mit-license.org/)
