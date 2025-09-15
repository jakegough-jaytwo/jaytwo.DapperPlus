using System;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.DapperPlus.Tests.Data;
using Moq;
using Xunit;

namespace jaytwo.DapperPlus.Tests.DataAccessTests;

public class QuerySingleOrDefaultAsyncTests
{
    [Fact]
    public async Task RunQuerySingleOrDefaultAsync_CommandText()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(commandText, default, default, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleOrDefaultAsync<MyRecord>(commandText);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleOrDefaultAsync_CommandText_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(commandText, default, default, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleOrDefaultAsync<MyRecord>(commandText, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleOrDefaultAsync_CommandText_Parameters()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(commandText, parameters, default, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleOrDefaultAsync<MyRecord>(commandText, parameters);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleOrDefaultAsync_CommandText_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = new Mock<DbTransaction>().Object;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(commandText, default, transaction, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleOrDefaultAsync<MyRecord>(commandText, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleOrDefaultAsync_CommandText_Parameters_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = new Mock<DbTransaction>().Object;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(commandText, parameters, transaction, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleOrDefaultAsync<MyRecord>(commandText, parameters, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleOrDefaultAsync_CommandText_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = new Mock<DbTransaction>().Object;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(commandText, default, transaction, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleOrDefaultAsync<MyRecord>(commandText, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleOrDefaultAsync_CommandText_Parameters_CancellationToken()
    {
        // arrange
        var parameters = new { Id = 1 };
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(commandText, parameters, default, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleOrDefaultAsync<MyRecord>(commandText, parameters, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleOrDefaultAsync_CommandText_Parameters_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = new Mock<DbTransaction>().Object;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(commandText, parameters, transaction, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleOrDefaultAsync<MyRecord>(commandText, parameters, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleOrDefaultAsync_all_arguments()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = new Mock<DbTransaction>().Object;
        var commandTimeoutSeconds = 10;
        var commandType = CommandType.Text;
        var cancellationTimeoutSeconds = 11;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };
        var prototype = expectedResult;

        Expression<Func<IDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, prototype, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleOrDefaultAsync<MyRecord>(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, prototype, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }
}
