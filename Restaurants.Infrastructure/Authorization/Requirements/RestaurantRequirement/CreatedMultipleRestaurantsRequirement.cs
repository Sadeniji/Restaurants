using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements.RestaurantRequirement;

public class CreatedMultipleRestaurantsRequirement(int minimumNumberOfRestaurants) : IAuthorizationRequirement
{
    public int MinimumNumberOfRestaurant { get; } = minimumNumberOfRestaurants;
}