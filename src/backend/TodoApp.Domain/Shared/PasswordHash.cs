namespace TodoApp.Domain.Shared;

public sealed record PasswordHash
{
    private PasswordHash(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static PasswordHash FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            throw new ArgumentException("Password hash cannot be null or whitespace", nameof(hash));
        }

        return new PasswordHash(hash);
    }

    public override string ToString() => Value;

    public static implicit operator string(PasswordHash hash) => hash.Value;
}
