using AuthService.Domain.Enums;

namespace AuthService.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public Role Role { get; set; } = Role.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
