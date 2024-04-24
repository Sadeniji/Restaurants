using MediatR;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public record DeleteRestaurantCommand(int Id) : IRequest;