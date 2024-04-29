using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDish;

public record DeleteDishByIdCommand(int RestaurantId, int DishId) : IRequest;