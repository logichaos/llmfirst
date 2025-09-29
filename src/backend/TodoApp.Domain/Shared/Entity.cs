namespace TodoApp.Domain.Shared;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    public TId Id { get; protected set; } = default!;

    public bool Equals(Entity<TId>? other)
        => other is not null && EqualityComparer<TId>.Default.Equals(Id, other.Id);

    public override bool Equals(object? obj)
        => obj is Entity<TId> other && Equals(other);

    public override int GetHashCode()
        => EqualityComparer<TId>.Default.GetHashCode(Id);

    protected static TId EnsureId(TId id)
        => id is null ? throw new ArgumentNullException(nameof(id)) : id;
}
