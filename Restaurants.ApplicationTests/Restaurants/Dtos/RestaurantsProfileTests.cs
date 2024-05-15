using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Restaurants.Dtos.Tests;

public class RestaurantsProfileTests
{
    private IMapper _mapper;
    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });

        _mapper = configuration.CreateMapper();
    }
    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // Arrange
        
        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "Test Restaurant",
            Category = "Category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            Address = new Address
            {
                City = "Test city",
                PostalCode = "123-45",
                Street = "Test street"
            }
        };

        // act
        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // Arrange
        
        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Restaurant",
            Category = "Category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            City = "Test city",
            PostalCode = "123-45",
            Street = "Test street"
        };

        // act
        var restaurant = _mapper.Map<Restaurant>(command);

        // assert
        restaurant.Should().NotBeNull();
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.Name.Should().Be(command.Name);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);
    }

}