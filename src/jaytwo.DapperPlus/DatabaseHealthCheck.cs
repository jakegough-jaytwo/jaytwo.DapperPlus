namespace jaytwo.DapperPlus;

public class DatabaseHealthCheck<TDapper>
    where TDapper : IDapperWrapper
{
    public DatabaseHealthCheck(TDapper dapperWrapper)
    {
        DapperWrapper = dapperWrapper;
    }

    protected virtual string TimestampQuery => "SELECT CURRENT_TIMESTAMP";

    protected TDapper DapperWrapper { get; }

    public virtual async Task<object> HealthCheckAsync(CancellationToken cancellationToken = default)
    {
        using var connection = DapperWrapper.CreateConnection();

        if (connection.ConnectionString.StartsWith("TODO"))
        {
            return "Disabled";
        }

        var connectionStringDetails = GetConnectionStringDetails(connection.ConnectionString);
        var result = new Dictionary<string, object>(connectionStringDetails);

        var exceptions = new List<Exception>();

        try
        {
            var nowTime = await DapperWrapper.ExecuteScalarAsync<DateTime>(TimestampQuery);
            result[TimestampQuery] = nowTime.ToString("O");
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }

        if (exceptions.Any())
        {
            var exceptionToThrow = exceptions.Count > 1
                ? new AggregateException("Multiple exceptions occurred during health check.", exceptions)
                : exceptions.Single();

            exceptionToThrow.Data.Add(nameof(result), result);
            throw exceptionToThrow;
        }

        return result;
    }

    protected virtual IDictionary<string, object> GetConnectionStringDetails(string connectionString)
        => new Dictionary<string, object>();
}
