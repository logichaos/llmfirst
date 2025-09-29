using System.Text.RegularExpressions;

namespace TodoApp.Domain.Shared;

public sealed partial record Email
{
    private static readonly Regex EmailRegex = CreateEmailRegex();

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty", nameof(value));
        }

        var normalized = value.Trim().ToLowerInvariant();
        if (!EmailRegex.IsMatch(normalized))
        {
            throw new ArgumentException("Email is not valid", nameof(value));
        }

        return new Email(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;

    [GeneratedRegex("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 1000)]
    private static partial Regex CreateEmailRegex();
}
