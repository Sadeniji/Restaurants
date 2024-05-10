using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDish;
using Restaurants.Application.Dishes.Commands.DeleteDishes;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetAllDishes;
using Restaurants.Application.Dishes.Queries.GetDishByRestaurantId;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
[ApiController]
[Authorize]
public class DishesController(IMediator _mediator) : Controller
{
    [HttpGet]
    [Authorize(Policy = PolicyName.AtLeast20Years)]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DishDto>))]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAll([FromRoute] int restaurantId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAllDishesQuery(restaurantId), cancellationToken));
    }

    [HttpGet("{dishId}")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DishDto>))]
    public async Task<ActionResult<DishDto>> GetDishesByRestaurantId([FromRoute] int restaurantId, [FromRoute] int dishId,  CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetDishByRestaurantIdQuery(restaurantId, dishId), cancellationToken));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, CreateDishCommand command, CancellationToken cancellationToken)
    {
        command.RestaurantId = restaurantId;
        var dishId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetDishesByRestaurantId), new { restaurantId, dishId}, null);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteDishes([FromRoute] int restaurantId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteDishesCommand(restaurantId), cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{dishId}")]
    public async Task<IActionResult> DeleteDish([FromRoute] int restaurantId, [FromRoute]int dishId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteDishByIdCommand(restaurantId, dishId), cancellationToken);
        return NoContent();
    }
}