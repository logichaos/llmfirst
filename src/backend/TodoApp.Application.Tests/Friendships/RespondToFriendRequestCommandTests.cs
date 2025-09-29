using FluentAssertions;
using TodoApp.Application.Friendships.Commands.RespondToFriendRequest;
using TodoApp.Application.Tests.Fakes;
using TodoApp.Domain.Friendships;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;
using Xunit;

namespace TodoApp.Application.Tests.Friendships;

public sealed class RespondToFriendRequestCommandTests
{
    private readonly FakeUserRepository _userRepository = new();
    private readonly FakeFriendshipRepository _friendshipRepository = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly User _alice;
    private readonly User _bob;
    private readonly Friendship _friendship;

    public RespondToFriendRequestCommandTests()
    {
        _alice = User.Register(Email.Create("alice@example.com"), "Alice", PasswordHash.FromHash("hash"));
        _bob = User.Register(Email.Create("bob@example.com"), "Bob", PasswordHash.FromHash("hash"));
        _userRepository.Add(_alice);
        _userRepository.Add(_bob);

        _friendship = Friendship.Request(_alice.Id, _bob.Id);
        _friendshipRepository.Add(_friendship);
    }

    [Fact]
    public async Task Handle_ShouldAcceptFriendship_WhenResponderIsAddressee()
    {
        var handler = new RespondToFriendRequestCommandHandler(_friendshipRepository, _userRepository, _unitOfWork);

        var result = await handler.Handle(new RespondToFriendRequestCommand(_friendship.Id.Value, _bob.Id.Value, true), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _friendship.Status.Should().Be(FriendshipStatus.Accepted);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenResponderNotAddressee()
    {
        var handler = new RespondToFriendRequestCommandHandler(_friendshipRepository, _userRepository, _unitOfWork);

        var result = await handler.Handle(new RespondToFriendRequestCommand(_friendship.Id.Value, _alice.Id.Value, true), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }
}
