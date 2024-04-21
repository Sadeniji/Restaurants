using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await restaurantsService.GetAllRestaurants(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantsService.GetRestaurantById(id, cancellationToken);

            return restaurant is null ? NotFound() : Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto createRestaurantDto, CancellationToken cancellationToken)
        {
            int id = await restaurantsService.CreateRestaurant(createRestaurantDto, cancellationToken);
            return CreatedAtAction(nameof(GetRestaurantById), new { id }, null);
        }
    }
}
