using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.RestaurantRequirement.Tests;

public class CreatedMultipleRestaurantsRequirementHandlerTests
{
    [Fact()]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new() { OwnerId = currentUser.Id },
            new() { OwnerId = currentUser.Id },
            new() { OwnerId = "2" }
        };

        var loggerMock = new Mock<ILogger<CreatedMultipleRestaurantsRequirement>>();
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(loggerMock.Object,
            userContextMock.Object,
            restaurantsRepositoryMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);
        
        // act
        await handler.HandleAsync(context);
        
        // assert
        context.HasSucceeded.Should().BeTrue();
    }
    
    [Fact()]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new() { OwnerId = currentUser.Id },
            new() { OwnerId = "2" }
        };

        var loggerMock = new Mock<ILogger<CreatedMultipleRestaurantsRequirement>>();
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(loggerMock.Object,
            userContextMock.Object,
            restaurantsRepositoryMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);
        
        // act
        await handler.HandleAsync(context);
        
        // assert
        context.HasSucceeded.Should().BeFalse();
    }

}