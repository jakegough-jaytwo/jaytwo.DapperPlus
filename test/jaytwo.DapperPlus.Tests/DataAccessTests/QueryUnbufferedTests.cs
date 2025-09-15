using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using jaytwo.DapperPlus.Tests.Data;
using Moq;
using Xunit;

namespace jaytwo.DapperPlus.Tests.DataAccessTests;

public class QueryUnbufferedTests
{
    [Fact]
    public void RunQueryUnbufferedAsync_CommandText_Transaction()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var transaction = new Mock<DbTransaction>().Object;
        var expectedResult = AsyncEnumerable.Empty<MyRecord>();

        Expression<Func<IDapperWrapper, IAsyncEnumerable<MyRecord>>> testExpression =
            x => x.QueryUnbufferedAsync<MyRecord>(commandText, default, transaction, default, default, default, default);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .Returns(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = dataAccess.RunQueryUnbufferedAsync<MyRecord>(commandText, transaction);

        // assert
        Assert.Same(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }

    [Fact]
    public void RunQueryUnbufferedAsync_all_arguments()
    {
        // arrange
        var commandText = Guid.NewGuid().ToString();
        var parameters = new { Id = 1 };
        var transaction = new Mock<DbTransaction>().Object;
        var commandTimeoutSeconds = 10;
        var commandType = CommandType.Text;
        var cancellationTimeoutSeconds = 11;
        var expectedResult = AsyncEnumerable.Empty<MyRecord>();
        var prototype = new MyRecord { Id = int.MaxValue };

        Expression<Func<IDapperWrapper, IAsyncEnumerable<MyRecord>>> testExpression =
            x => x.QueryUnbufferedAsync<MyRecord>(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, prototype);

        var mockDapper = new Mock<IDapperWrapper>();
        mockDapper
            .Setup(testExpression)
            .Returns(expectedResult);

        var dataAccess = new MyDataAccess(mockDapper.Object);

        // act
        var actualResult = dataAccess.RunQueryUnbufferedAsync<MyRecord>(commandText, parameters, transaction, commandTimeoutSeconds, commandType, cancellationTimeoutSeconds, prototype);

        // assert
        Assert.Same(expectedResult, actualResult);
        mockDapper.Verify(testExpression, Times.Once);
    }
}
