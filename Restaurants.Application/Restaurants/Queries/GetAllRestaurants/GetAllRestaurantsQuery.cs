using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public record GetAllRestaurantsQuery(string? SearchPhrase, 
    int PageNumber, 
    int PageSize, 
    string? SortBy,
    SortDirection SortDirection) : IRequest<PagedResult<RestaurantDto>>;