namespace TodoApp.Domain.Shared;

public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());

    public static UserId FromGuid(Guid value)
        => value == Guid.Empty ? throw new ArgumentException("UserId cannot be empty", nameof(value)) : new UserId(value);

    public static UserId Empty => new(Guid.Empty);

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserId id) => id.Value;
}
