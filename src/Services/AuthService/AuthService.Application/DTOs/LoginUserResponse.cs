﻿namespace AuthService.Application.DTOs;

public class LoginUserResponse
{
    public string AccessToken { get; set; } = null!;
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Role { get; set; } = null!;
}
