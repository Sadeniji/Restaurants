using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantId;

public record GetRestaurantByIdQuery(int Id) : IRequest<RestaurantDto?>;