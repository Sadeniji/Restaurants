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

    public async Task<Restaurant?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Restaurants.Include(d => d.Dishes).FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}