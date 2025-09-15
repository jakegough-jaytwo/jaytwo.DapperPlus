using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace jaytwo.DapperPlus;

public class GridReaderWrapper : IGridReaderWrapper, IDisposable, IAsyncDisposable
{
    private readonly DbConnection _connection;
    private readonly GridReader _gridReader;
    private readonly bool _disposeConnection;

    public GridReaderWrapper(DbConnection connection, GridReader gridReader, bool disposeConnection)
    {
        _connection = connection;
        _gridReader = gridReader;
        _disposeConnection = disposeConnection;
    }

    public async Task<T?> ReadFirstAsync<T>()
        => await _gridReader.ReadFirstAsync<T>();

    public async Task<T?> ReadFirstOrDefaultAsync<T>()
        => await _gridReader.ReadFirstOrDefaultAsync<T>();

    public async Task<T> ReadSingleAsync<T>()
        => await _gridReader.ReadSingleAsync<T>();

    public async Task<T?> ReadSingleOrDefaultAsync<T>()
        => await _gridReader.ReadSingleOrDefaultAsync<T>();

    public async Task<IEnumerable<T>> ReadAsync<T>(bool buffered = true)
        => await _gridReader.ReadAsync<T>(buffered);

    public IAsyncEnumerable<T> ReadUnbufferedAsync<T>()
        => _gridReader.ReadUnbufferedAsync<T>();

    public void Dispose()
    {
        if (_disposeConnection)
        {
            _connection.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposeConnection)
        {
            await _connection.DisposeAsync();
        }
    }
}
