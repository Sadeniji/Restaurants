using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetAllDishes;

public record GetAllDishesQuery(int RestaurantId) : IRequest<IEnumerable<DishDto>>;