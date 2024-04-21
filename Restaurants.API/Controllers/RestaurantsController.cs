using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantId;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetAllRestaurantsQuery(), cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id), cancellationToken);
            return restaurant is null ? NotFound() : Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand createRestaurantCommand, CancellationToken cancellationToken)
        {
            int id = await mediator.Send(createRestaurantCommand, cancellationToken);
            return CreatedAtAction(nameof(GetRestaurantById), new { id }, null);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new DeleteRestaurantCommand(id), cancellationToken);
            return response  ? Ok(response) : NotFound();
        }
    }
}
