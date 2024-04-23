using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantId;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDto>))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetAllRestaurantsQuery(), cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto?>> GetRestaurantById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id), cancellationToken);
            return restaurant is null ? NotFound() : Ok(restaurant);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand createRestaurantCommand, CancellationToken cancellationToken)
        {
            int id = await mediator.Send(createRestaurantCommand, cancellationToken);
            return CreatedAtAction(nameof(GetRestaurantById), new { id }, null);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id, CancellationToken cancellationToken)
        {
            var isDeeleted = await mediator.Send(new DeleteRestaurantCommand(id), cancellationToken);
            return isDeeleted  ? Ok(isDeeleted) : NotFound();
        }
        
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantCommand updateRestaurantCommand, CancellationToken cancellationToken)
        {
            updateRestaurantCommand = updateRestaurantCommand with { Id = id };
            var isUpdated = await mediator.Send(updateRestaurantCommand, cancellationToken);
            return isUpdated ? NoContent() : NotFound();
        }
    }
}
