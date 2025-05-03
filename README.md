# jaytwo.DapperPlus

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

| Package |   |   |
|---------|---|---|
| `jaytwo.DapperPlus` | [![NuGet Version](https://img.shields.io/nuget/v/jaytwo.DapperPlus.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/jaytwo.DapperPlus) | [![NuGet Downloads](https://img.shields.io/nuget/dt/jaytwo.DapperPlus.svg?style=flat)](https://www.nuget.org/packages/jaytwo.DapperPlus) |
| `jaytwo.DapperPlus.Postgres` | [![NuGet Version](https://img.shields.io/nuget/v/jaytwo.DapperPlus.Postgres.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/jaytwo.DapperPlus.Postgres) | [![NuGet Downloads](https://img.shields.io/nuget/dt/jaytwo.DapperPlus.Postgres.svg?style=flat)](https://www.nuget.org/packages/jaytwo.DapperPlus.Postgres) |
| `jaytwo.DapperPlus.MySql` | [![NuGet Version](https://img.shields.io/nuget/v/jaytwo.DapperPlus.MySql.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/jaytwo.DapperPlus.MySql) | [![NuGet Downloads](https://img.shields.io/nuget/dt/jaytwo.DapperPlus.MySql.svg?style=flat)](https://www.nuget.org/packages/jaytwo.DapperPlus.MySql) |
| `jaytwo.DapperPlus.SqlServer` | [![NuGet Version](https://img.shields.io/nuget/v/jaytwo.DapperPlus.SqlServer.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/jaytwo.DapperPlus.SqlServer) | [![NuGet Downloads](https://img.shields.io/nuget/dt/jaytwo.DapperPlus.SqlServer.svg?style=flat)](https://www.nuget.org/packages/jaytwo.DapperPlus.SqlServer) |

### TL;DR

This project exists because I wanted to use Dapper with a `CancellationToken`.

### The Longer Story

Dapper is already great—I've been a huge fan for years. But I ran into trouble on a REST API project where database performance issues led to clients retrying HTTP requests after their own timeouts. The real problem? The server was still waiting for the original request to finish—meaning the database connection stayed open and the heavy query kept running. From the client’s perspective, the old request was done and dusted, but on the backend, the load just kept compounding.

I figured this would be easy to solve: add a `CancellationToken` parameter to controller methods (which automatically ties to client disconnects), and propagate that token down to the DB calls.

...or so I thought.

Dapper _does_ support using a `CancellationToken`—but only on obscure overloads that require a `CommandDefinition`, not the convenient method signatures we’re all used to. So I built my own overloads—the ones I wish Dapper included out of the box.

Over time, this evolved. I added telemetry, standardized patterns, and turned it into my go-to base layer for data access. Eventually, I published this package to improve testing and stop all the copy-pasting.

I know this may go against Dapper’s minimalist spirit. Dapper is meant to be a lightweight, stateless micro-ORM for people who want to write SQL without full ADO boilerplate. This project adds connection factories and base classes—getting a little close to that “EF context” smell.

But here’s the thing: I **like** writing clean, explicit SQL. I want full control over DTOs, transactions, and connection lifetimes. I don’t want the magic of `TransactionScope` or EF’s automatic change tracking.

Most of the time, yeah, I just want a list of results. But sometimes? I want to crack open a good old-fashioned `DataReader`. This wrapper gives me that flexibility—without having to manually build `DbCommand` objects and manage connection lifetimes myself.


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




---

Made with &hearts; by Jake
