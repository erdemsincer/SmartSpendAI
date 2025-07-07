using AuthService.Application.Commands;
using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Handlers;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = _tokenService.GenerateToken(user);

        return new LoginUserResponse
        {
            AccessToken = token,
            UserId = user.Id,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role.ToString()
        };
    }
}
