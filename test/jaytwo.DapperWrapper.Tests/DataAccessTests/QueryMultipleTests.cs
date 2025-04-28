using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using jaytwo.DapperWrapper.Tests.Data;
using Moq;
using Xunit;
using static Dapper.SqlMapper;

namespace jaytwo.DapperWrapper.Tests.DataAccessTests;

public class QueryMultipleTests
{
    [Fact]
    public async Task RunQueryMultipleAsync_CommandText()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var expectedResult = default(GridReader)!;

        Expression<Func<IDapperWrapper, Task<GridReader>>> testExpression =
            x => x.QueryMultipleAsync(commandText, default, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQueryMultipleAsync(commandText);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQueryMultipleAsync_CommandText_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = default(GridReader)!;

        Expression<Func<IDapperWrapper, Task<GridReader>>> testExpression =
            x => x.QueryMultipleAsync(commandText, default, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQueryMultipleAsync(commandText, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQueryMultipleAsync_CommandText_Parameters()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var expectedResult = default(GridReader)!;

        Expression<Func<IDapperWrapper, Task<GridReader>>> testExpression =
            x => x.QueryMultipleAsync(commandText, parameters, default, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQueryMultipleAsync(commandText, parameters);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQueryMultipleAsync_CommandText_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = new Mock<DbTransaction>().Object;
        var expectedResult = default(GridReader)!;

        Expression<Func<IDapperWrapper, Task<GridReader>>> testExpression =
            x => x.QueryMultipleAsync(commandText, default, transaction, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQueryMultipleAsync(commandText, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQueryMultipleAsync_CommandText_Parameters_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = new Mock<DbTransaction>().Object;
        var expectedResult = default(GridReader)!;

        Expression<Func<IDapperWrapper, Task<GridReader>>> testExpression =
            x => x.QueryMultipleAsync(commandText, parameters, transaction, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQueryMultipleAsync(commandText, parameters, transaction);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQueryMultipleAsync_CommandText_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = new Mock<DbTransaction>().Object;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = default(GridReader)!;

        Expression<Func<IDapperWrapper, Task<GridReader>>> testExpression =
            x => x.QueryMultipleAsync(commandText, default, transaction, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQueryMultipleAsync(commandText, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQueryMultipleAsync_CommandText_Parameters_CancellationToken()
    {
        // arrange
        var parameters = new { Id = 1 };
        var commandText = Guid.NewGuid().ToString();
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = default(GridReader)!;

        Expression<Func<IDapperWrapper, Task<GridReader>>> testExpression =
            x => x.QueryMultipleAsync(commandText, parameters, default, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQueryMultipleAsync(commandText, parameters, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQueryMultipleAsync_CommandText_Parameters_Transaction_CancellationToken()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = new Mock<DbTransaction>().Object;
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var expectedResult = default(GridReader)!;

        Expression<Func<IDapperWrapper, Task<GridReader>>> testExpression =
            x => x.QueryMultipleAsync(commandText, parameters, transaction, default, default, default, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQueryMultipleAsync(commandText, parameters, transaction, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public async Task RunQueryMultipleAsync_all_arguments()
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
        var expectedResult = default(GridReader)!;

        Expression<Func<IDapperWrapper, Task<GridReader>>> testExpression =
            x => x.QueryMultipleAsync(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, cancellationToken);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .ReturnsAsync(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = await dataAccess.RunQueryMultipleAsync(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, cancellationToken);

        // assert
        Assert.Equal(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }
}
