using FluentAssertions;
using TodoApp.Domain.Friendships;
using TodoApp.Domain.Friendships.Events;
using TodoApp.Domain.Shared;
using Xunit;

namespace TodoApp.Domain.Tests.Friendships;

public class FriendshipTests
{
    [Fact]
    public void Create_ShouldStartPendingAndRaiseEvent()
    {
        var requesterId = UserId.New();
        var addresseeId = UserId.New();

        var friendship = Friendship.Request(requesterId, addresseeId);

        friendship.Status.Should().Be(FriendshipStatus.Pending);
        friendship.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<FriendRequestSentDomainEvent>();
    }

    [Fact]
    public void Accept_ShouldChangeStatusAndRaiseEvent()
    {
        var requesterId = UserId.New();
        var addresseeId = UserId.New();
        var friendship = Friendship.Request(requesterId, addresseeId);

        friendship.Accept(addresseeId);

        friendship.Status.Should().Be(FriendshipStatus.Accepted);
        friendship.DomainEvents.Should().ContainSingle(e => e is FriendRequestAcceptedDomainEvent);
    }

    [Fact]
    public void Accept_FromNonAddressee_ShouldThrow()
    {
        var friendship = Friendship.Request(UserId.New(), UserId.New());

        var act = () => friendship.Accept(UserId.New());

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Reject_ShouldChangeStatus()
    {
        var requesterId = UserId.New();
        var addresseeId = UserId.New();
        var friendship = Friendship.Request(requesterId, addresseeId);

        friendship.Reject(addresseeId);

        friendship.Status.Should().Be(FriendshipStatus.Rejected);
        friendship.DomainEvents.Should().ContainSingle(e => e is FriendRequestRejectedDomainEvent);
    }
}
