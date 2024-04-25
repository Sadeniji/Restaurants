using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDish;

public class DeleteDishByIdCommandHandler(ILogger<DeleteDishByIdCommand> logger,
                                          IRestaurantsRepository restaurantsRepository,
                                          IDishesRepository dishesRepository) : IRequestHandler<DeleteDishByIdCommand>
{
    public async Task Handle(DeleteDishByIdCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting dish with id: {DishId} from restaurant with Id: {RestaurantId}", 
                              request.DishId, request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId, cancellationToken);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        var dishToDelete = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);
        
        if (dishToDelete is null)
        {
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        }
        await dishesRepository.DeleteByDishId(dishToDelete, cancellationToken);
    }
}