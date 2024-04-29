using MediatR;

namespace Restaurants.Application.Users.Commands;

public record UpdateUserDetailsCommand(DateOnly? DateOfBirth, string? Nationality) : IRequest;