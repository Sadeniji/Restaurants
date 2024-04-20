using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync(CancellationToken cancellation);

    Task<Restaurant?> GetByIdAsync(int id, CancellationToken cancellationToken);
}