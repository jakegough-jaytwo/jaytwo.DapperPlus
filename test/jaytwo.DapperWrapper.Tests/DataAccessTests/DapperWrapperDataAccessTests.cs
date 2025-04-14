using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Moq;
using Xunit;

namespace jaytwo.DapperWrapper.Tests.DataAccessTests;

public class DapperWrapperDataAccessTests
{
    [Fact]
    public void CreateConnection()
    {
        // arrange
        var expectedResult = It.IsAny<DbConnection>();

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(x => x.CreateConnection())
            .Returns(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = dataAccess.RunCreateConnection();

        // assert
        Assert.Same(expectedResult, actualResult);
        mockDapper.Verify(x => x.CreateConnection(), Times.Once);
    }

    [Fact]
    public async Task HealthCheckAsync()
    {
        // arrange
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = new { Status = "OK" };

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(x => x.HealthCheckAsync(cancellationToken))
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.HealthCheckAsync(cancellationToken);

        // assert
        Assert.Same(expectedResult, actualResult);
        mockDapper.Verify(x => x.HealthCheckAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RunInTransactionAsync()
    {
        // arrange
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var isolationLevel = IsolationLevel.ReadCommitted;

        Func<DbTransaction, Task> callback = transaction => Task.CompletedTask;
        Expression<Func<IDapperWrapper, Task>> testExpression =
            x => x.RunInTransactionAsync(callback, isolationLevel, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .Returns(Task.CompletedTask);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        await dataAccess.RunInTransactionAsync(callback, isolationLevel, cancellationToken);

        // assert
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task CommitTransactionAsync()
    {
        // arrange
        var transaction = It.IsAny<DbTransaction>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        Expression<Func<IDapperWrapper, Task>> testExpression =
            x => x.CommitTransactionAsync(transaction, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .Returns(Task.CompletedTask);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        await dataAccess.RunCommitTransactionAsync(transaction, cancellationToken);

        // assert
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RollbackTransactionAsync()
    {
        // arrange
        var transaction = It.IsAny<DbTransaction>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        Expression<Func<IDapperWrapper, Task>> testExpression =
            x => x.RollbackTransactionAsync(transaction, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .Returns(Task.CompletedTask);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        await dataAccess.RunRollbackTransactionAsync(transaction, cancellationToken);

        // assert
        mockDapper.Verify(testExpression, Times.Once);
    }
}
