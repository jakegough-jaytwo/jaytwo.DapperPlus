using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace jaytwo.DapperPlus.Tests.Data;

public class MyDataAccess : DapperWrapperDataAccess
{
    public MyDataAccess(IDapperWrapper dapper)
        : base(dapper)
    {
    }

    public DbConnection RunCreateConnection()
        => CreateConnection();

    public async Task RunCommitTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken)
        => await CommitTransactionAsync(transaction, cancellationToken);

    public async Task RunRollbackTransactionAsync(DbTransaction transaction, CancellationToken cancellationToken)
        => await RollbackTransactionAsync(transaction, cancellationToken);

    public async Task<int> RunExecuteAsync(string command)
        => await ExecuteAsync(command);

    public async Task<int> RunExecuteAsync(string command, object parameters)
        => await ExecuteAsync(command, parameters);

    public async Task<int> RunExecuteAsync(string command, DbTransaction transaction)
        => await ExecuteAsync(command, transaction);

    public async Task<int> RunExecuteAsync(string command, object parameters, DbTransaction transaction)
        => await ExecuteAsync(command, parameters, transaction);

    public async Task<int> RunExecuteAsync(string command, CancellationToken cancellationToken)
        => await ExecuteAsync(command, cancellationToken);

    public async Task<int> RunExecuteAsync(string command, object parameters, CancellationToken cancellationToken)
        => await ExecuteAsync(command, parameters, cancellationToken);

    public async Task<int> RunExecuteAsync(string command, DbTransaction transaction, CancellationToken cancellationToken)
        => await ExecuteAsync(command, transaction, cancellationToken);

    public async Task<int> RunExecuteAsync(string command, object parameters, DbTransaction transaction, CancellationToken cancellationToken)
        => await ExecuteAsync(command, parameters, transaction, cancellationToken);

    public async Task<int> RunExecuteAsync(
        string command,
        object? parameters,
        DbTransaction? transaction,
        int? commandTimeoutSeconds,
        CommandType? commandType,
        int? cancellationTimeoutSeconds,
        CancellationToken cancellationToken)
        => await ExecuteAsync(
            command,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            cancellationToken);

    public async Task<T?> RunExecuteScalarAsync<T>(string command)
        => await ExecuteScalarAsync<T>(command);

    public async Task<T?> RunExecuteScalarAsync<T>(string command, object parameters)
        => await ExecuteScalarAsync<T>(command, parameters);

    public async Task<T?> RunExecuteScalarAsync<T>(string command, DbTransaction transaction)
        => await ExecuteScalarAsync<T>(command, transaction);

    public async Task<T?> RunExecuteScalarAsync<T>(string command, object parameters, DbTransaction transaction)
        => await ExecuteScalarAsync<T>(command, parameters, transaction);

    public async Task<T?> RunExecuteScalarAsync<T>(string command, CancellationToken cancellationToken)
        => await ExecuteScalarAsync<T>(command, cancellationToken);

    public async Task<T?> RunExecuteScalarAsync<T>(string command, object parameters, CancellationToken cancellationToken)
        => await ExecuteScalarAsync<T>(command, parameters, cancellationToken);

    public async Task<T?> RunExecuteScalarAsync<T>(string command, DbTransaction transaction, CancellationToken cancellationToken)
        => await ExecuteScalarAsync<T>(command, transaction, cancellationToken);

    public async Task<T?> RunExecuteScalarAsync<T>(string command, object parameters, DbTransaction transaction, CancellationToken cancellationToken)
        => await ExecuteScalarAsync<T>(command, parameters, transaction, cancellationToken);

    public async Task<T?> RunExecuteScalarAsync<T>(
        string command,
        object? parameters,
        DbTransaction? transaction,
        int? commandTimeoutSeconds,
        CommandType? commandType,
        int? cancellationTimeoutSeconds,
        CancellationToken cancellationToken)
        => await ExecuteScalarAsync<T>(
            command,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            cancellationToken);

    public async Task<T?> RunQuerySingleOrDefaultAsync<T>(string command)
        => await QuerySingleOrDefaultAsync<T>(command);

    public async Task<T?> RunQuerySingleOrDefaultAsync<T>(string command, object parameters)
        => await QuerySingleOrDefaultAsync<T>(command, parameters);

    public async Task<T?> RunQuerySingleOrDefaultAsync<T>(string command, DbTransaction transaction)
        => await QuerySingleOrDefaultAsync<T>(command, transaction);

    public async Task<T?> RunQuerySingleOrDefaultAsync<T>(string command, object parameters, DbTransaction transaction)
        => await QuerySingleOrDefaultAsync<T>(command, parameters, transaction);

    public async Task<T?> RunQuerySingleOrDefaultAsync<T>(string command, CancellationToken cancellationToken)
        => await QuerySingleOrDefaultAsync<T>(command, cancellationToken);

    public async Task<T?> RunQuerySingleOrDefaultAsync<T>(string command, object parameters, CancellationToken cancellationToken)
        => await QuerySingleOrDefaultAsync<T>(command, parameters, cancellationToken);

    public async Task<T?> RunQuerySingleOrDefaultAsync<T>(string command, DbTransaction transaction, CancellationToken cancellationToken)
        => await QuerySingleOrDefaultAsync<T>(command, transaction, cancellationToken);

    public async Task<T?> RunQuerySingleOrDefaultAsync<T>(string command, object parameters, DbTransaction transaction, CancellationToken cancellationToken)
        => await QuerySingleOrDefaultAsync<T>(command, parameters, transaction, cancellationToken);

    public async Task<T?> RunQuerySingleOrDefaultAsync<T>(
        string command,
        object? parameters,
        DbTransaction? transaction,
        int? commandTimeoutSeconds,
        CommandType? commandType,
        int? cancellationTimeoutSeconds,
        T? prototype,
        CancellationToken cancellationToken)
        => await QuerySingleOrDefaultAsync(
            command,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            prototype,
            cancellationToken);

    public async Task<T> RunQuerySingleAsync<T>(string command)
        => await QuerySingleAsync<T>(command);

    public async Task<T> RunQuerySingleAsync<T>(string command, object parameters)
        => await QuerySingleAsync<T>(command, parameters);

    public async Task<T> RunQuerySingleAsync<T>(string command, DbTransaction transaction)
        => await QuerySingleAsync<T>(command, transaction);

    public async Task<T> RunQuerySingleAsync<T>(string command, object parameters, DbTransaction transaction)
        => await QuerySingleAsync<T>(command, parameters, transaction);

    public async Task<T> RunQuerySingleAsync<T>(string command, CancellationToken cancellationToken)
        => await QuerySingleAsync<T>(command, cancellationToken);

    public async Task<T> RunQuerySingleAsync<T>(string command, object parameters, CancellationToken cancellationToken)
        => await QuerySingleAsync<T>(command, parameters, cancellationToken);

    public async Task<T> RunQuerySingleAsync<T>(string command, DbTransaction transaction, CancellationToken cancellationToken)
        => await QuerySingleAsync<T>(command, transaction, cancellationToken);

    public async Task<T> RunQuerySingleAsync<T>(string command, object parameters, DbTransaction transaction, CancellationToken cancellationToken)
        => await QuerySingleAsync<T>(command, parameters, transaction, cancellationToken);

    public async Task<T> RunQuerySingleAsync<T>(
        string command,
        object? parameters,
        DbTransaction? transaction,
        int? commandTimeoutSeconds,
        CommandType? commandType,
        int? cancellationTimeoutSeconds,
        T? prototype,
        CancellationToken cancellationToken)
        => await QuerySingleAsync(
            command,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            prototype,
            cancellationToken);

    public async Task<IList<T>> RunQueryAsync<T>(string command)
        => await QueryAsync<T>(command);

    public async Task<IList<T>> RunQueryAsync<T>(string command, object parameters)
        => await QueryAsync<T>(command, parameters);

    public async Task<IList<T>> RunQueryAsync<T>(string command, DbTransaction transaction)
        => await QueryAsync<T>(command, transaction);

    public async Task<IList<T>> RunQueryAsync<T>(string command, object parameters, DbTransaction transaction)
        => await QueryAsync<T>(command, parameters, transaction);

    public async Task<IList<T>> RunQueryAsync<T>(string command, CancellationToken cancellationToken)
        => await QueryAsync<T>(command, cancellationToken);

    public async Task<IList<T>> RunQueryAsync<T>(string command, object parameters, CancellationToken cancellationToken)
        => await QueryAsync<T>(command, parameters, cancellationToken);

    public async Task<IList<T>> RunQueryAsync<T>(string command, DbTransaction transaction, CancellationToken cancellationToken)
        => await QueryAsync<T>(command, transaction, cancellationToken);

    public async Task<IList<T>> RunQueryAsync<T>(string command, object parameters, DbTransaction transaction, CancellationToken cancellationToken)
        => await QueryAsync<T>(command, parameters, transaction, cancellationToken);

    public async Task<IList<T>> RunQueryAsync<T>(
        string command,
        object? parameters,
        DbTransaction? transaction,
        int? commandTimeoutSeconds,
        CommandType? commandType,
        int? cancellationTimeoutSeconds,
        T? prototype,
        CancellationToken cancellationToken)
        => await QueryAsync(
            command,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            prototype,
            cancellationToken);

    public async Task<IGridReaderWrapper> RunQueryMultipleAsync(string command)
        => await QueryMultipleAsync(command);

    public async Task<IGridReaderWrapper> RunQueryMultipleAsync(string command, object parameters)
        => await QueryMultipleAsync(command, parameters);

    public async Task<IGridReaderWrapper> RunQueryMultipleAsync(string command, DbTransaction transaction)
        => await QueryMultipleAsync(command, transaction);

    public async Task<IGridReaderWrapper> RunQueryMultipleAsync(string command, object parameters, DbTransaction transaction)
        => await QueryMultipleAsync(command, parameters, transaction);

    public async Task<IGridReaderWrapper> RunQueryMultipleAsync(string command, CancellationToken cancellationToken)
        => await QueryMultipleAsync(command, cancellationToken);

    public async Task<IGridReaderWrapper> RunQueryMultipleAsync(string command, object parameters, CancellationToken cancellationToken)
        => await QueryMultipleAsync(command, parameters, cancellationToken);

    public async Task<IGridReaderWrapper> RunQueryMultipleAsync(string command, DbTransaction transaction, CancellationToken cancellationToken)
        => await QueryMultipleAsync(command, transaction, cancellationToken);

    public async Task<IGridReaderWrapper> RunQueryMultipleAsync(string command, object parameters, DbTransaction transaction, CancellationToken cancellationToken)
        => await QueryMultipleAsync(command, parameters, transaction, cancellationToken);

    public async Task<IGridReaderWrapper> RunQueryMultipleAsync(
        string command,
        object? parameters,
        DbTransaction? transaction,
        int? commandTimeoutSeconds,
        CommandType? commandType,
        int? cancellationTimeoutSeconds,
        CancellationToken cancellationToken)
        => await QueryMultipleAsync(
            command,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            cancellationToken);

    public async Task<IDataReader> RunExecuteReaderAsync(string command)
        => await ExecuteReaderAsync(command);

    public async Task<IDataReader> RunExecuteReaderAsync(string command, object parameters)
        => await ExecuteReaderAsync(command, parameters);

    public async Task<IDataReader> RunExecuteReaderAsync(string command, DbTransaction transaction)
        => await ExecuteReaderAsync(command, transaction);

    public async Task<IDataReader> RunExecuteReaderAsync(string command, object parameters, DbTransaction transaction)
        => await ExecuteReaderAsync(command, parameters, transaction);

    public async Task<IDataReader> RunExecuteReaderAsync(string command, CancellationToken cancellationToken)
        => await ExecuteReaderAsync(command, cancellationToken);

    public async Task<IDataReader> RunExecuteReaderAsync(string command, object parameters, CancellationToken cancellationToken)
        => await ExecuteReaderAsync(command, parameters, cancellationToken);

    public async Task<IDataReader> RunExecuteReaderAsync(string command, DbTransaction transaction, CancellationToken cancellationToken)
        => await ExecuteReaderAsync(command, transaction, cancellationToken);

    public async Task<IDataReader> RunExecuteReaderAsync(string command, object parameters, DbTransaction transaction, CancellationToken cancellationToken)
        => await ExecuteReaderAsync(command, parameters, transaction, cancellationToken);

    public async Task<IDataReader> RunExecuteReaderAsync(
        string command,
        object? parameters,
        DbTransaction? transaction,
        int? commandTimeoutSeconds,
        CommandType? commandType,
        int? cancellationTimeoutSeconds,
        CommandBehavior? commandBehavior,
        CancellationToken cancellationToken)
        => await ExecuteReaderAsync(
            command,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            commandBehavior,
            cancellationToken);

    public IAsyncEnumerable<T> RunQueryUnbufferedAsync<T>(string command, DbTransaction transaction)
        => QueryUnbufferedAsync<T>(command, transaction);

    public IAsyncEnumerable<T> RunQueryUnbufferedAsync<T>(
        string command,
        object? parameters,
        DbTransaction? transaction,
        int? commandTimeoutSeconds,
        CommandType? commandType,
        int? cancellationTimeoutSeconds,
        T? prototype)
        => QueryUnbufferedAsync(
            command,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            prototype);
}
