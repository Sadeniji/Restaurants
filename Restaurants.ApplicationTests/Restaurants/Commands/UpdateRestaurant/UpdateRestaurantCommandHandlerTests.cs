using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _logggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;
    public UpdateRestaurantCommandHandlerTests()
    {
        _logggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        
        _handler = new UpdateRestaurantCommandHandler(_logggerMock.Object,
            _restaurantsRepositoryMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }
    
    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurant()
    {
        // arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand(restaurantId, "New test", "New Description", true);

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test"
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId, CancellationToken.None))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);
        
        // act
        await _handler.Handle(command, CancellationToken.None);
        
        _restaurantsRepositoryMock.Verify(r => r.SaveChanges(CancellationToken.None), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
    }
    
    [Fact()]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // arrange
        var restaurantId = 2;
        var command = new UpdateRestaurantCommand(restaurantId, "New test", "New Description", true);
        
        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId, CancellationToken.None))
            .ReturnsAsync((Restaurant?)null);
        
        // act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
       
        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id: {restaurantId} doesn't exist");
    }
    
    [Fact()]
    public async Task Handle_WithUnAuthorizeUser_ShouldThrowForbidException()
    {
        // arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand(restaurantId, "New test", "New Description", true);

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test"
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId, CancellationToken.None))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, ResourceOperation.Update))
            .Returns(false);
        
        // act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
       
        // Assert
        await act.Should().ThrowAsync<ForbiddenException>();
    }
}