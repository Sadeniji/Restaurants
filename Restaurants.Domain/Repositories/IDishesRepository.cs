using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{   
    Task<int> Create(Dish dish, CancellationToken cancellationToken);
    Task Delete(List<Dish> dishes, CancellationToken cancellationToken);
    Task DeleteByDishId(Dish dish, CancellationToken cancellationToken);
}