using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Commands;

public class LoginUserCommand : IRequest<LoginUserResponse>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
