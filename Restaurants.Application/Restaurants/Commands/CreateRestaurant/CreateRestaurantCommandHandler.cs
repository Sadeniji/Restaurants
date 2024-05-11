using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
                                            IMapper mapper,
                                            IRestaurantsRepository restaurantsRepository,
                                            IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("{UserEmail} {UserId} is creating a new restaurant {@Restaurant}", currentUser.Email, currentUser.Id, request);

        var newRestaurant = mapper.Map<Restaurant>(request);
        newRestaurant.OwnerId = currentUser.Id;
        return await restaurantsRepository.Create(newRestaurant, cancellationToken);
    }
}