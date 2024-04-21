using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

public class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger, IMapper mapper) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants(CancellationToken cancellation)
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync(cancellation);

        return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }

    public async Task<RestaurantDto?> GetRestaurantById(int id, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting restaurant with id {id}");
        var restaurant = await restaurantsRepository.GetByIdAsync(id, cancellationToken);

        return mapper.Map<RestaurantDto?>(restaurant);
    }
}