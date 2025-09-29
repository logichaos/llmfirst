using TodoApp.Domain.Friendships.Events;
using TodoApp.Domain.Shared;

namespace TodoApp.Domain.Friendships;

public sealed class Friendship : AggregateRoot<FriendshipId>
{
    private Friendship(FriendshipId id, UserId requesterId, UserId addresseeId, FriendshipStatus status, DateTimeOffset createdAt)
    {
        if (requesterId == addresseeId)
        {
            throw new DomainException("Users cannot become friends with themselves");
        }

        Id = id;
        RequesterId = requesterId;
        AddresseeId = addresseeId;
        Status = status;
        CreatedAt = createdAt;
    }

    public UserId RequesterId { get; }
    public UserId AddresseeId { get; }
    public FriendshipStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? RespondedAt { get; private set; }

    public static Friendship Request(UserId requesterId, UserId addresseeId)
    {
        var friendship = new Friendship(FriendshipId.New(), requesterId, addresseeId, FriendshipStatus.Pending, DateTimeOffset.UtcNow);
        friendship.RaiseDomainEvent(new FriendRequestSentDomainEvent(friendship.Id, friendship.RequesterId, friendship.AddresseeId));
        return friendship;
    }

    public void Accept(UserId byUser)
    {
        EnsurePendingResponse(byUser);
        Status = FriendshipStatus.Accepted;
        RespondedAt = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new FriendRequestAcceptedDomainEvent(Id, RequesterId, AddresseeId));
    }

    public void Reject(UserId byUser)
    {
        EnsurePendingResponse(byUser);
        Status = FriendshipStatus.Rejected;
        RespondedAt = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new FriendRequestRejectedDomainEvent(Id, RequesterId, AddresseeId));
    }

    private void EnsurePendingResponse(UserId byUser)
    {
        if (!AddresseeId.Equals(byUser))
        {
            throw new DomainException("Only the addressee can respond to a friendship request");
        }

        if (Status != FriendshipStatus.Pending)
        {
            throw new DomainException("Friendship request is no longer pending");
        }
    }

    public static Friendship Restore(
        FriendshipId id,
        UserId requesterId,
        UserId addresseeId,
        FriendshipStatus status,
        DateTimeOffset createdAt,
        DateTimeOffset? respondedAt)
    {
        var friendship = new Friendship(id, requesterId, addresseeId, status, createdAt)
        {
            RespondedAt = respondedAt
        };

        friendship.ClearDomainEvents();
        return friendship;
    }
}
