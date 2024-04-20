using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    Task<IEnumerable<RestaurantDto>> GetAllRestaurants(CancellationToken cancellation);
    Task<RestaurantDto?> GetRestaurantById(int id, CancellationToken cancellationToken);
}