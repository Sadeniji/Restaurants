using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(
    ILogger<CreateDishCommandHandler> logger,
                                      IMapper _mapper,
                                      IRestaurantsRepository restaurantsRepository,
                                      IDishesRepository dishesRepository) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new dish: {@DishRequest}", command);
        var restaurant = await restaurantsRepository.GetByIdAsync(command.RestaurantId, cancellationToken);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), command.RestaurantId.ToString());
        }
    
        var dish = _mapper.Map<Dish>(command);
        
       return await dishesRepository.Create(dish, cancellationToken);
    }
}