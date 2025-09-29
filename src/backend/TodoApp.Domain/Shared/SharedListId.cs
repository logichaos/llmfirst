namespace TodoApp.Domain.Shared;

public readonly record struct SharedListId(Guid Value)
{
    public static SharedListId New() => new(Guid.NewGuid());

    public static SharedListId FromGuid(Guid value)
        => value == Guid.Empty ? throw new ArgumentException("SharedListId cannot be empty", nameof(value)) : new SharedListId(value);

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(SharedListId id) => id.Value;
}
