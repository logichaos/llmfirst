using TodoApp.Domain.Shared;

namespace TodoApp.Domain.Friendships.Events;

public sealed record FriendRequestAcceptedDomainEvent(FriendshipId FriendshipId, UserId RequesterId, UserId AddresseeId) : DomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
