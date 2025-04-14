using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Moq;
using Xunit;

namespace jaytwo.DapperWrapper.Tests;

public class DapperWrapperDataAccessTests
{
    public DapperWrapperDataAccessTests()
    {
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

    [Fact]
    public async Task RunExecuteScalarAsync_CommandText()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var expectedResult = double.Pi;

        Expression<Func<IDapperWrapper, Task<double>>> testExpression =
            x => x.ExecuteScalarAsync<double>(commandText, default, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteScalarAsync<double>(commandText);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteScalarAsync_CommandText_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = double.Pi;

        Expression<Func<IDapperWrapper, Task<double>>> testExpression =
            x => x.ExecuteScalarAsync<double>(commandText, default, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteScalarAsync<double>(commandText, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteScalarAsync_CommandText_Parameters()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var expectedResult = double.Pi;

        Expression<Func<IDapperWrapper, Task<double>>> testExpression =
            x => x.ExecuteScalarAsync<double>(commandText, parameters, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteScalarAsync<double>(commandText, parameters);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteScalarAsync_CommandText_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = It.IsNotNull<DbTransaction>();
        var expectedResult = double.Pi;

        Expression<Func<IDapperWrapper, Task<double>>> testExpression =
            x => x.ExecuteScalarAsync<double>(commandText, default, transaction, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteScalarAsync<double>(commandText, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteScalarAsync_CommandText_Parameters_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = It.IsNotNull<DbTransaction>();
        var expectedResult = double.Pi;

        Expression<Func<IDapperWrapper, Task<double>>> testExpression =
            x => x.ExecuteScalarAsync<double>(commandText, parameters, transaction, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteScalarAsync<double>(commandText, parameters, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteScalarAsync_CommandText_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = It.IsNotNull<DbTransaction>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = double.Pi;

        Expression<Func<IDapperWrapper, Task<double>>> testExpression =
            x => x.ExecuteScalarAsync<double>(commandText, default, transaction, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteScalarAsync<double>(commandText, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteScalarAsync_CommandText_Parameters_CancellationToken()
    {
        // arrange
        var parameters = new { Id = 1 };
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = double.Pi;

        Expression<Func<IDapperWrapper, Task<double>>> testExpression =
            x => x.ExecuteScalarAsync<double>(commandText, parameters, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteScalarAsync<double>(commandText, parameters, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteScalarAsync_CommandText_Parameters_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = It.IsNotNull<DbTransaction>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = double.Pi;

        Expression<Func<IDapperWrapper, Task<double>>> testExpression =
            x => x.ExecuteScalarAsync<double>(commandText, parameters, transaction, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteScalarAsync<double>(commandText, parameters, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunExecuteScalarAsync_all_arguments()
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
        var expectedResult = double.Pi;

        Expression<Func<IDapperWrapper, Task<double>>> testExpression =
            x => x.ExecuteScalarAsync<double>(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunExecuteScalarAsync<double>(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    private class MyDataAccess : DapperWrapperDataAccess
    {
        public MyDataAccess(IDapperWrapper dapper)
            : base(dapper)
        {
        }

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
    }
}
