using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
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
        int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection, CancellationToken cancellation)
    {
        var searchPhraseToLower = searchPhrase?.ToLower();

        var baseQuery = dbContext.Restaurants
            .Where(r => searchPhraseToLower == null || r.Name.ToLower().Contains(searchPhraseToLower)
                                                    || r.Description.ToLower().Contains(searchPhraseToLower));//.Include(d => d.Dishes);

        var totalCount = await baseQuery.CountAsync(cancellation);

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category },
            };

            var selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending 
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);

        }
        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Include(d => d.Dishes)
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