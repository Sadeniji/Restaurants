using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;
using FluentAssertions;

namespace Restaurants.API.Middlewares.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact()]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleWare = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        // act
        await middleWare.InvokeAsync(context, nextDelegateMock.Object);

        // assert
        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }

    [Fact()]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCodeTo404()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleWare = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");

        // act
        await middleWare.InvokeAsync(context, _ => throw notFoundException);

        // assert
        context.Response.StatusCode.Should().Be(404);
    }

    [Fact()]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCodeTo403()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleWare = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new ForbiddenException();

        // act
        await middleWare.InvokeAsync(context, _ => throw exception);

        // assert
        context.Response.StatusCode.Should().Be(403);
    }

    [Fact()]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCodeTo500()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleWare = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new Exception();

        // act
        await middleWare.InvokeAsync(context, _ => throw exception);

        // assert
        context.Response.StatusCode.Should().Be(500);
    }
}