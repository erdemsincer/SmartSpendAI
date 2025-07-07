using AuthService.Application.Commands;
using AuthService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };

        var userId = await _mediator.Send(command);
        return Ok(new { userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var command = new LoginUserCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
