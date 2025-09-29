namespace TodoApp.Domain.Shared;

public readonly record struct FriendshipId(Guid Value)
{
    public static FriendshipId New() => new(Guid.NewGuid());

    public static FriendshipId FromGuid(Guid value)
        => value == Guid.Empty ? throw new ArgumentException("FriendshipId cannot be empty", nameof(value)) : new FriendshipId(value);

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(FriendshipId id) => id.Value;
}
