using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public record DeleteDishesCommand(int RestaurantId) : IRequest;