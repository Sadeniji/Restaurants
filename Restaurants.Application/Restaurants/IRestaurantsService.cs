using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    Task<IEnumerable<RestaurantDto>> GetAllRestaurants(CancellationToken cancellation);
    Task<RestaurantDto?> GetRestaurantById(int id, CancellationToken cancellationToken);
}