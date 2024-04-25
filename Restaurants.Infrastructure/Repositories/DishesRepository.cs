using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<int> Create(Dish dish, CancellationToken cancellationToken)
    {
        dbContext.Dishes.Add(dish);
        await dbContext.SaveChangesAsync(cancellationToken);
        return dish.Id;
    }

    public async Task Delete(List<Dish> dishes, CancellationToken cancellationToken)
    {
        dbContext.Dishes.RemoveRange(dishes);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByDishId(Dish dish, CancellationToken cancellationToken)
    {
        dbContext.Dishes.Remove(dish);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}