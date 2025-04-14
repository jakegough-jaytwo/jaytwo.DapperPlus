using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Moq;
using Xunit;

namespace jaytwo.DapperWrapper.Tests.DataAccessTests;

public class ExecuteAsyncTests
{
    [Fact]
    public async Task RunExecuteAsync_CommandText()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var expectedResult = int.MinValue;

        Expression<Func<IDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(commandText, default, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteAsync(commandText);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteAsync_CommandText_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = int.MinValue;

        Expression<Func<IDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(commandText, default, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteAsync(commandText, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteAsync_CommandText_Parameters()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var expectedResult = int.MinValue;

        Expression<Func<IDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(commandText, parameters, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteAsync(commandText, parameters);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteAsync_CommandText_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = It.IsNotNull<DbTransaction>();
        var expectedResult = int.MinValue;

        Expression<Func<IDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(commandText, default, transaction, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteAsync(commandText, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteAsync_CommandText_Parameters_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = It.IsNotNull<DbTransaction>();
        var expectedResult = int.MinValue;

        Expression<Func<IDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(commandText, parameters, transaction, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteAsync(commandText, parameters, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteAsync_CommandText_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = It.IsNotNull<DbTransaction>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = int.MinValue;

        Expression<Func<IDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(commandText, default, transaction, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteAsync(commandText, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteAsync_CommandText_Parameters_CancellationToken()
    {
        // arrange
        var parameters = new { Id = 1 };
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = int.MinValue;

        Expression<Func<IDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(commandText, parameters, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteAsync(commandText, parameters, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteAsync_CommandText_Parameters_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = It.IsNotNull<DbTransaction>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = int.MinValue;

        Expression<Func<IDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(commandText, parameters, transaction, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteAsync(commandText, parameters, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteAsync_all_arguments()
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
        var expectedResult = int.MinValue;

        Expression<Func<IDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteAsync(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }
}
