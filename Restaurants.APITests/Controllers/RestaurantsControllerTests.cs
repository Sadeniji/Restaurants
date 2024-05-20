using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.APITests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _applicationFactory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> applicationFactory)
    {
        _applicationFactory = applicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                    _ => _restaurantsRepositoryMock.Object));
            });
        });
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200k()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();

        // act
        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact()]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();

        // act
        var result = await client.GetAsync("/api/restaurants");

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact()]
    public async Task GetById_ForNonExistingId_ShouldReturns404NotFound()
    {
        // Arrange
        var id = 1123;

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id, CancellationToken.None))
            .ReturnsAsync((Restaurant?)null);

        var client = _applicationFactory.CreateClient();

        // act
        var result = await client.GetAsync($"/api/restaurants/{id}");

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    //[Fact()]
    //public async Task GetById_ForExistingId_ShouldReturns200Ok()
    //{
    //    // Arrange
    //    var id = 99;

    //    var restaurant = new Restaurant()
    //    {
    //        Id = id,
    //        Name = "Test",
    //        Description = "Test description"
    //    };

    //    _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id, CancellationToken.None))
    //        .ReturnsAsync(restaurant);

    //    var client = _applicationFactory.CreateClient();

    //    // act
    //    var result = await client.GetAsync($"/api/restaurants/{id}");
    //    var restaurantDto = await result.Content.ReadFromJsonAsync<RestaurantDto>();

    //    // assert
    //    result.StatusCode.Should().Be(HttpStatusCode.OK);
    //    restaurantDto.Should().NotBeNull();
    //    restaurantDto.Name.Should().Be("Test");
    //}
}