using MediatR;

namespace Restaurants.Application.Users.Commands.UpdateUserDetails;

public record UpdateUserDetailsCommand(DateOnly? DateOfBirth, string? Nationality) : IRequest;