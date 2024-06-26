﻿using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesCommandHandler(ILogger<DeleteDishesCommandHandler> logger, 
                                        IRestaurantsRepository restaurantsRepository,
                                        IDishesRepository dishesRepository,
                                        IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishesCommand>
{
    public async Task Handle(DeleteDishesCommand command, CancellationToken cancellationToken)
    {
        logger.LogWarning("Deleting all dishes from restaurant: {RestaurantId}", command.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(command.RestaurantId, cancellationToken);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), command.RestaurantId.ToString());
        }

        if (restaurant.Dishes == null || !restaurant.Dishes.Any())
        {
            throw new NotFoundException(nameof(Dish), command.RestaurantId.ToString());
        }
        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
        {
            throw new ForbiddenException();
        }

        await dishesRepository.Delete(restaurant.Dishes, cancellationToken);
    }
}