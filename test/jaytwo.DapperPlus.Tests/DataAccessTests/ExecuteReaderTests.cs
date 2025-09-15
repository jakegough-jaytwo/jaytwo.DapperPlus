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

public class ExecuteReaderTests
{
    [Fact]
    public async Task RunExecuteReaderAsync_CommandText()
    {
        // arrange
        var connection = Mock.Of<DbConnection>();
        var commandText = Guid.NewGuid().ToString();
        var expectedResult = Mock.Of<IDataReader>();

        Expression<Func<IDapperWrapper, Task<IDataReader>>> testExpression =
            x => x.ExecuteReaderAsync(commandText, default, default, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteReaderAsync(commandText);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteReaderAsync_CommandText_CancellationToken()
    {
        // arrange
        var connection = Mock.Of<DbConnection>();
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = Mock.Of<IDataReader>();

        Expression<Func<IDapperWrapper, Task<IDataReader>>> testExpression =
            x => x.ExecuteReaderAsync(commandText, default, default, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteReaderAsync(commandText, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteReaderAsync_CommandText_Parameters()
    {
        // arrange
        var connection = Mock.Of<DbConnection>();
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var expectedResult = Mock.Of<IDataReader>();

        Expression<Func<IDapperWrapper, Task<IDataReader>>> testExpression =
            x => x.ExecuteReaderAsync(commandText, parameters, default, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteReaderAsync(commandText, parameters);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteReaderAsync_CommandText_Transaction()
    {
        // arrange
        var connection = Mock.Of<DbConnection>();
        var commandText = Guid.NewGuid().ToString();
        var transaction = new Mock<DbTransaction>().Object;
        var expectedResult = Mock.Of<IDataReader>();

        Expression<Func<IDapperWrapper, Task<IDataReader>>> testExpression =
            x => x.ExecuteReaderAsync(commandText, default, transaction, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteReaderAsync(commandText, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteReaderAsync_CommandText_Parameters_Transaction()
    {
        // arrange
        var connection = Mock.Of<DbConnection>();
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = new Mock<DbTransaction>().Object;
        var expectedResult = Mock.Of<IDataReader>();

        Expression<Func<IDapperWrapper, Task<IDataReader>>> testExpression =
            x => x.ExecuteReaderAsync(commandText, parameters, transaction, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteReaderAsync(commandText, parameters, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteReaderAsync_CommandText_Transaction_CancellationToken()
    {
        // arrange
        var connection = Mock.Of<DbConnection>();
        var commandText = Guid.NewGuid().ToString();
        var transaction = new Mock<DbTransaction>().Object;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = Mock.Of<IDataReader>();

        Expression<Func<IDapperWrapper, Task<IDataReader>>> testExpression =
            x => x.ExecuteReaderAsync(commandText, default, transaction, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteReaderAsync(commandText, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteReaderAsync_CommandText_Parameters_CancellationToken()
    {
        // arrange
        var connection = Mock.Of<DbConnection>();
        var parameters = new { Id = 1 };
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = Mock.Of<IDataReader>();

        Expression<Func<IDapperWrapper, Task<IDataReader>>> testExpression =
            x => x.ExecuteReaderAsync(commandText, parameters, default, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteReaderAsync(commandText, parameters, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteReaderAsync_CommandText_Parameters_Transaction_CancellationToken()
    {
        // arrange
        var connection = Mock.Of<DbConnection>();
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = new Mock<DbTransaction>().Object;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = Mock.Of<IDataReader>();

        Expression<Func<IDapperWrapper, Task<IDataReader>>> testExpression =
            x => x.ExecuteReaderAsync(commandText, parameters, transaction, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteReaderAsync(commandText, parameters, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteReaderAsync_all_arguments()
    {
        // arrange
        var connection = Mock.Of<DbConnection>();
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = new Mock<DbTransaction>().Object;
        var commandTimeoutSeconds = 10;
        var commandType = CommandType.Text;
        var cancellationTimeoutSeconds = 11;
        using var cancellationTokenSource = new CancellationTokenSource();
        var commandBehavior = CommandBehavior.SequentialAccess;
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = Mock.Of<IDataReader>();

        Expression<Func<IDapperWrapper, Task<IDataReader>>> testExpression =
            x => x.ExecuteReaderAsync(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, commandBehavior, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteReaderAsync(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, commandBehavior, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }
}
