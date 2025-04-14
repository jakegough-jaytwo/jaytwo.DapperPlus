using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Moq;
using Xunit;

namespace jaytwo.DapperWrapper.Tests.DataAccessTests;

public class QuerySingleAsyncTests
{
    [Fact]
    public async Task RunQuerySingleAsync_CommandText()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(commandText, default, default, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleAsync<MyRecord>(commandText);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleAsync_CommandText_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(commandText, default, default, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleAsync<MyRecord>(commandText, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleAsync_CommandText_Parameters()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(commandText, parameters, default, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleAsync<MyRecord>(commandText, parameters);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleAsync_CommandText_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = It.IsNotNull<DbTransaction>();
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(commandText, default, transaction, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleAsync<MyRecord>(commandText, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleAsync_CommandText_Parameters_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = It.IsNotNull<DbTransaction>();
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(commandText, parameters, transaction, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleAsync<MyRecord>(commandText, parameters, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleAsync_CommandText_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = It.IsNotNull<DbTransaction>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(commandText, default, transaction, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleAsync<MyRecord>(commandText, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleAsync_CommandText_Parameters_CancellationToken()
    {
        // arrange
        var parameters = new { Id = 1 };
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(commandText, parameters, default, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleAsync<MyRecord>(commandText, parameters, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleAsync_CommandText_Parameters_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = It.IsNotNull<DbTransaction>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(commandText, parameters, transaction, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleAsync<MyRecord>(commandText, parameters, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQuerySingleAsync_all_arguments()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = It.IsNotNull<DbTransaction>();
        var commandTimeoutSeconds = 10;
        var commandType = CommandType.Text;
        var cancellationTimeoutSeconds = 11;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new MyRecord { Id = int.MaxValue };
        var prototype = expectedResult;

        Expression<Func<IDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, prototype, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQuerySingleAsync<MyRecord>(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, prototype, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }
}
