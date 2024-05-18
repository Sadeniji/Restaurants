using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger,     
                                            IRestaurantsRepository restaurantsRepository,
                                            IMapper mapper,
                                            IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdateRestaurant}", command.Id, command);
        var restaurant = await restaurantsRepository.GetByIdAsync(command.Id, cancellationToken);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(restaurant), command.Id.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
        {
            throw new ForbiddenException();
        }

        mapper.Map(command, restaurant);
        await restaurantsRepository.SaveChanges(cancellationToken);
    }
}