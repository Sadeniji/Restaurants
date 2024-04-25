using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishByRestaurantId;

public record GetDishByRestaurantIdQuery(int RestaurantId, int DishId): IRequest<DishDto>;