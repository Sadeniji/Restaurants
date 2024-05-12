using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync(CancellationToken cancellation);
    Task<(List<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(string? searchPhrase, int pageSize,
        int pageNumber, string? sortBy, SortDirection sortDirection, CancellationToken cancellation);
    Task<Restaurant?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> Create(Restaurant newRestaurant, CancellationToken cancellationToken);
    Task Delete(Restaurant restaurant, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}