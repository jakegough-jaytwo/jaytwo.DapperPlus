using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jaytwo.DapperPlus;

public interface IGridReaderWrapper
    : IDisposable, IAsyncDisposable
{
    Task<T?> ReadFirstAsync<T>();

    Task<T?> ReadFirstOrDefaultAsync<T>();

    Task<T> ReadSingleAsync<T>();

    Task<T?> ReadSingleOrDefaultAsync<T>();

    Task<IEnumerable<T>> ReadAsync<T>(bool buffered = true);

    IAsyncEnumerable<T> ReadUnbufferedAsync<T>();
}
