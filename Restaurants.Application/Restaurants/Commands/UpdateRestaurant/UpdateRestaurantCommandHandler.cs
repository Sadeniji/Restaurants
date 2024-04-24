using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommand> logger, 
                                            IRestaurantsRepository restaurantsRepository,
                                            IMapper mapper) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdateRestaurant}", command.Id, command);
        var restaurant = await restaurantsRepository.GetByIdAsync(command.Id, cancellationToken);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(restaurant), command.Id.ToString());
        }
        
        mapper.Map(command, restaurant);
        await restaurantsRepository.SaveChanges(cancellationToken);
    }
}