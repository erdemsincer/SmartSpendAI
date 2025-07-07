using MediatR;

namespace AuthService.Application.Commands;

public class RegisterUserCommand : IRequest<Guid>  
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
