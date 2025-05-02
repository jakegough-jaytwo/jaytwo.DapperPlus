using System;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using jaytwo.DapperWrapper.Tests.Data;
using Moq;
using Xunit;
using static Dapper.SqlMapper;
using static jaytwo.DapperWrapper.Tests.DapperWrapperTests.DapperWrapperTests;

namespace jaytwo.DapperWrapper.Tests.DapperWrapperTests;

public class DapperWrapperTests
{
    [Fact]
    public void CreateConnection()
    {
        // Arrange
        var connection = Mock.Of<DbConnection>();
        var dapperWrapper = new DefaultDapperWrapper(() => connection, default);

        // Act
        var result = dapperWrapper.CreateConnection();

        // Assert
        Assert.Same(result, connection);
    }

    [Theory]
    [InlineData(CommandType.Text)]
    [InlineData(CommandType.StoredProcedure)]
    public async Task ExecuteAsync(CommandType commandType)
    {
        // Arrange
        var connection = Mock.Of<DbConnection>();
        var transaction = Mock.Of<DbTransaction>();
        var cancellationToken = new CancellationTokenSource().Token;

        var connectionFactory = () => connection;
        var commandText = "hello world";
        var parameters = new { Id = 1 };
        var commandTimeoutSeconds = 3;
        var cancellationTimeoutSeconds = 5;
        var expectedResult = 7;

        Expression<Func<DefaultDapperWrapper, Task<int>>> testExpression =
            x => x.ExecuteAsync(
                It.Is<DapperCommandContext>(x =>
                    x.CommandText == commandText
                    && x.Parameters == parameters
                    && x.Transaction == transaction
                    && x.CommandTimeoutSeconds == commandTimeoutSeconds
                    && x.CommandType == commandType
                    && x.CancellationTimeoutSeconds == cancellationTimeoutSeconds),
                cancellationToken);

        var dapperWrapper = new Mock<DefaultDapperWrapper>(connectionFactory) { CallBase = true };
        dapperWrapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await dapperWrapper.Object.ExecuteAsync(
            commandText,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            cancellationToken);

        // Assert
        Assert.Equal(expectedResult, actualResult);
        dapperWrapper.Verify(testExpression, Times.Once);
    }

    [Theory]
    [InlineData(CommandType.Text)]
    [InlineData(CommandType.StoredProcedure)]
    public async Task ExecuteScalarAsync(CommandType commandType)
    {
        // Arrange
        var connection = Mock.Of<DbConnection>();
        var transaction = Mock.Of<DbTransaction>();
        var cancellationToken = new CancellationTokenSource().Token;

        var connectionFactory = () => connection;
        var commandText = "hello world";
        var parameters = new { Id = 1 };
        var commandTimeoutSeconds = 3;
        var cancellationTimeoutSeconds = 5;
        var expectedResult = "7";

        Expression<Func<DefaultDapperWrapper, Task<string?>>> testExpression =
            x => x.ExecuteScalarAsync<string>(
                It.Is<DapperCommandContext>(x =>
                    x.CommandText == commandText
                    && x.Parameters == parameters
                    && x.Transaction == transaction
                    && x.CommandTimeoutSeconds == commandTimeoutSeconds
                    && x.CommandType == commandType
                    && x.CancellationTimeoutSeconds == cancellationTimeoutSeconds),
                cancellationToken);

        var dapperWrapper = new Mock<DefaultDapperWrapper>(connectionFactory) { CallBase = true };
        dapperWrapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await dapperWrapper.Object.ExecuteScalarAsync<string>(
            commandText,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            cancellationToken);

        // Assert
        Assert.Equal(expectedResult, actualResult);
        dapperWrapper.Verify(testExpression, Times.Once);
    }

    [Theory]
    [InlineData(CommandType.Text)]
    [InlineData(CommandType.StoredProcedure)]
    public async Task QuerySingleAsync(CommandType commandType)
    {
        // Arrange
        var connection = Mock.Of<DbConnection>();
        var transaction = Mock.Of<DbTransaction>();
        var cancellationToken = new CancellationTokenSource().Token;

        var connectionFactory = () => connection;
        var commandText = "hello world";
        var parameters = new { Id = 1 };
        var commandTimeoutSeconds = 3;
        var cancellationTimeoutSeconds = 5;
        var expectedResult = new MyRecord() { Id = 7 };
        var prototype = expectedResult;

        Expression<Func<DefaultDapperWrapper, Task<MyRecord>>> testExpression =
            x => x.QuerySingleAsync<MyRecord>(
                It.Is<DapperCommandContext>(x =>
                    x.CommandText == commandText
                    && x.Parameters == parameters
                    && x.Transaction == transaction
                    && x.CommandTimeoutSeconds == commandTimeoutSeconds
                    && x.CommandType == commandType
                    && x.CancellationTimeoutSeconds == cancellationTimeoutSeconds),
                cancellationToken);

        var dapperWrapper = new Mock<DefaultDapperWrapper>(connectionFactory) { CallBase = true };
        dapperWrapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await dapperWrapper.Object.QuerySingleAsync<MyRecord>(
            commandText,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            prototype,
            cancellationToken);

        // Assert
        Assert.Same(expectedResult, actualResult);
        dapperWrapper.Verify(testExpression, Times.Once);
    }

    [Theory]
    [InlineData(CommandType.Text)]
    [InlineData(CommandType.StoredProcedure)]
    public async Task QuerySingleOrDefaultAsync(CommandType commandType)
    {
        // Arrange
        var connection = Mock.Of<DbConnection>();
        var transaction = Mock.Of<DbTransaction>();
        var cancellationToken = new CancellationTokenSource().Token;

        var connectionFactory = () => connection;
        var commandText = "hello world";
        var parameters = new { Id = 1 };
        var commandTimeoutSeconds = 3;
        var cancellationTimeoutSeconds = 5;
        var expectedResult = new MyRecord() { Id = 7 };
        var prototype = expectedResult;

        Expression<Func<DefaultDapperWrapper, Task<MyRecord?>>> testExpression =
            x => x.QuerySingleOrDefaultAsync<MyRecord>(
                It.Is<DapperCommandContext>(x =>
                    x.CommandText == commandText
                    && x.Parameters == parameters
                    && x.Transaction == transaction
                    && x.CommandTimeoutSeconds == commandTimeoutSeconds
                    && x.CommandType == commandType
                    && x.CancellationTimeoutSeconds == cancellationTimeoutSeconds),
                cancellationToken);

        var dapperWrapper = new Mock<DefaultDapperWrapper>(connectionFactory) { CallBase = true };
        dapperWrapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await dapperWrapper.Object.QuerySingleOrDefaultAsync<MyRecord>(
            commandText,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            prototype,
            cancellationToken);

        // Assert
        Assert.Same(expectedResult, actualResult);
        dapperWrapper.Verify(testExpression, Times.Once);
    }

    [Theory]
    [InlineData(CommandType.Text)]
    [InlineData(CommandType.StoredProcedure)]
    public async Task QueryAsync(CommandType commandType)
    {
        // Arrange
        var connection = Mock.Of<DbConnection>();
        var transaction = Mock.Of<DbTransaction>();
        var cancellationToken = new CancellationTokenSource().Token;

        var connectionFactory = () => connection;
        var commandText = "hello world";
        var parameters = new { Id = 1 };
        var commandTimeoutSeconds = 3;
        var cancellationTimeoutSeconds = 5;
        var expectedResult = new[] { new MyRecord() { Id = 7 } };
        var prototype = expectedResult.Single();

        Expression<Func<DefaultDapperWrapper, Task<IList<MyRecord>>>> testExpression =
            x => x.QueryAsync<MyRecord>(
                It.Is<DapperCommandContext>(x =>
                    x.CommandText == commandText
                    && x.Parameters == parameters
                    && x.Transaction == transaction
                    && x.CommandTimeoutSeconds == commandTimeoutSeconds
                    && x.CommandType == commandType
                    && x.CancellationTimeoutSeconds == cancellationTimeoutSeconds),
                cancellationToken);

        var dapperWrapper = new Mock<DefaultDapperWrapper>(connectionFactory) { CallBase = true };
        dapperWrapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await dapperWrapper.Object.QueryAsync<MyRecord>(
            commandText,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            prototype,
            cancellationToken);

        // Assert
        Assert.Same(expectedResult, actualResult);
        dapperWrapper.Verify(testExpression, Times.Once);
    }

    [Theory]
    [InlineData(CommandType.Text)]
    [InlineData(CommandType.StoredProcedure)]
    public async Task QueryMultipleAsync(CommandType commandType)
    {
        // Arrange
        var connection = Mock.Of<DbConnection>();
        var transaction = Mock.Of<DbTransaction>();
        var cancellationToken = new CancellationTokenSource().Token;

        var connectionFactory = () => connection;
        var commandText = "hello world";
        var parameters = new { Id = 1 };
        var commandTimeoutSeconds = 3;
        var cancellationTimeoutSeconds = 5;
        var expectedResult = Mock.Of<IGridReaderWrapper>();

        Expression<Func<DefaultDapperWrapper, Task<IGridReaderWrapper>>> testExpression =
            x => x.QueryMultipleAsync(
                It.Is<DapperCommandContext>(x =>
                    x.CommandText == commandText
                    && x.Parameters == parameters
                    && x.Transaction == transaction
                    && x.CommandTimeoutSeconds == commandTimeoutSeconds
                    && x.CommandType == commandType
                    && x.CancellationTimeoutSeconds == cancellationTimeoutSeconds),
                cancellationToken);

        var dapperWrapper = new Mock<DefaultDapperWrapper>(connectionFactory) { CallBase = true };
        dapperWrapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        // Act
        var actualResult = await dapperWrapper.Object.QueryMultipleAsync(
            commandText,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            cancellationToken);

        // Assert
        Assert.Same(expectedResult, actualResult);
        dapperWrapper.Verify(testExpression, Times.Once);
    }

    [Theory]
    [InlineData(CommandType.Text)]
    [InlineData(CommandType.StoredProcedure)]
    public void QueryUnbufferedAsync(CommandType commandType)
    {
        // Arrange
        var connection = Mock.Of<DbConnection>();
        var transaction = Mock.Of<DbTransaction>();
        var cancellationToken = new CancellationTokenSource().Token;

        var connectionFactory = () => connection;
        var commandText = "hello world";
        var parameters = new { Id = 1 };
        var commandTimeoutSeconds = 3;
        var cancellationTimeoutSeconds = 5;
        var expectedResult = AsyncEnumerable.Empty<MyRecord>();
        var prototype = new MyRecord() { Id = 7 };

        Expression<Func<DefaultDapperWrapper, IAsyncEnumerable<MyRecord>>> testExpression =
            x => x.QueryUnbufferedAsync<MyRecord>(
                It.Is<DapperCommandContext>(x =>
                    x.CommandText == commandText
                    && x.Parameters == parameters
                    && x.Transaction == transaction
                    && x.CommandTimeoutSeconds == commandTimeoutSeconds
                    && x.CommandType == commandType
                    && x.CancellationTimeoutSeconds == cancellationTimeoutSeconds));

        var dapperWrapper = new Mock<DefaultDapperWrapper>(connectionFactory) { CallBase = true };
        dapperWrapper
            .Setup(testExpression)
            .Returns(expectedResult);

        // Act
        var actualResult = dapperWrapper.Object.QueryUnbufferedAsync<MyRecord>(
            commandText,
            parameters,
            transaction,
            commandTimeoutSeconds,
            commandType,
            cancellationTimeoutSeconds,
            prototype);

        // Assert
        Assert.Same(expectedResult, actualResult);
        dapperWrapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task QueryUnbufferedAsync_cancellationToken_is_preserved()
    {
        // Arrange
        var connection = new Mock<DbConnection>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var connectionFactory = () => connection.Object;

        var dapperWrapper = new DefaultDapperWrapper(connectionFactory);

        var enumerator = dapperWrapper.QueryUnbufferedAsync(
            x => new FakeAsyncEnumerable<MyRecord>(cancellationToken),
            Mock.Of<DapperCommandContext>(),
            default)
            .WithCancellation(cancellationToken)
            .GetAsyncEnumerator();

        // Act
        cancellationTokenSource.Cancel(); // Cancel before move next

        // Assert
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await enumerator.MoveNextAsync();
        });
    }

    public class FakeAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly CancellationToken _expectedToken;

        public FakeAsyncEnumerable(CancellationToken expectedToken)
        {
            _expectedToken = expectedToken;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            // Here you can assert the token matches what you expect, or react to cancellation
            return new FakeAsyncEnumerator<T>(_expectedToken, cancellationToken);
        }
    }

    public class FakeAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly CancellationToken _linkedToken;
        private readonly CancellationToken _outerToken;

        public FakeAsyncEnumerator(CancellationToken expectedToken, CancellationToken cancellationToken)
        {
            _linkedToken = cancellationToken;
            _outerToken = expectedToken;
        }

        public T Current => default!;

        public async ValueTask<bool> MoveNextAsync()
        {
            // Simulate async work
            await Task.Delay(10, _linkedToken);

            _linkedToken.ThrowIfCancellationRequested();
            _outerToken.ThrowIfCancellationRequested();

            return false; // No elements
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
