using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommand> logger, 
                                            IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantCommand, bool>
{
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleting restaurant with id {request.Id}");
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (restaurant is null)
        {
            return false;
        }
        await restaurantsRepository.Delete(restaurant, cancellationToken);
        return true;
    }
}