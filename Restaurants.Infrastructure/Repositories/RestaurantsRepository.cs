using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync(CancellationToken cancellation)
    {
        return await dbContext.Restaurants.Include(d => d.Dishes).ToListAsync(cancellation);
    }

    public async Task<(List<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(string? searchPhrase,
        int pageSize, int pageNumber, CancellationToken cancellation)
    {
        var searchPhraseToLower = searchPhrase?.ToLower();
        var baseQuery = dbContext.Restaurants
            .Where(r => searchPhraseToLower == null || r.Name.ToLower().Contains(searchPhraseToLower)
                                                    || r.Description.ToLower().Contains(searchPhraseToLower)).Include(d => d.Dishes);

        var totalCount = baseQuery.Count();

        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync(cancellation);

        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Restaurants.Include(d => d.Dishes).FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<int> Create(Restaurant newRestaurant, CancellationToken cancellationToken)
    {
        await dbContext.Restaurants.AddAsync(newRestaurant, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return newRestaurant.Id;
    }

    public async Task Delete(Restaurant restaurant, CancellationToken cancellationToken)
    {
        dbContext.Remove(restaurant);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChanges(CancellationToken cancellationToken) => await dbContext.SaveChangesAsync(cancellationToken);
}