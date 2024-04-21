using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    private readonly List<string> _validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];
    public CreateRestaurantDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(dto => dto.Category)
            .Must(_validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories");
            // .Custom((value, context) =>
            // {
            //     var isValidCategory = _validCategories.Contains(value);
            //     if (isValidCategory)
            //     {
            //         context.AddFailure("Category", "Invalid category. Please choose from the valid categories");
            //     }
            // });
        
        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address.");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX).");
    }
}