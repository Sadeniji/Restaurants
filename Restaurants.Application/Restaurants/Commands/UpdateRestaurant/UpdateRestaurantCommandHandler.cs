using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommand> logger, 
                                            IRestaurantsRepository restaurantsRepository,
                                            IMapper mapper) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updating restaurant with {command.Id}");
        var restaurant = await restaurantsRepository.GetByIdAsync(command.Id, cancellationToken);

        if (restaurant is null)
        {
            return false;
        }
        
        mapper.Map(command, restaurant);
        await restaurantsRepository.SaveChanges(cancellationToken);
        return true;
    }
}