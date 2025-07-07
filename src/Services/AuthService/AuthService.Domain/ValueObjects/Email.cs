namespace AuthService.Domain.ValueObjects;

public record Email
{
    public string Value { get; init; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            throw new ArgumentException("Invalid email address.");

        Value = value.Trim().ToLower();
    }

    public static Email Create(string email) => new(email);

    public override string ToString() => Value;
}
