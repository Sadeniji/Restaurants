using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class DishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public DishCommandValidator()
    {
        RuleFor(dish => dish.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a non-negative number.");
        
        RuleFor(dish => dish.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Kilo calories must be a non-negative number.");
    }
}