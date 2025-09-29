using FluentAssertions;
using TodoApp.Application.Friendships.Commands.SendFriendRequest;
using TodoApp.Application.Tests.Fakes;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;
using Xunit;

namespace TodoApp.Application.Tests.Friendships;

public sealed class SendFriendRequestCommandTests
{
    private readonly FakeUserRepository _userRepository = new();
    private readonly FakeFriendshipRepository _friendshipRepository = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly User _alice;
    private readonly User _bob;

    public SendFriendRequestCommandTests()
    {
        _alice = User.Register(Email.Create("alice@example.com"), "Alice", PasswordHash.FromHash("hash"));
        _bob = User.Register(Email.Create("bob@example.com"), "Bob", PasswordHash.FromHash("hash"));
        _userRepository.Add(_alice);
        _userRepository.Add(_bob);
    }

    [Fact]
    public async Task Handle_ShouldCreateFriendship_WhenValid()
    {
        var handler = new SendFriendRequestCommandHandler(_userRepository, _friendshipRepository, _unitOfWork);

        var result = await handler.Handle(new SendFriendRequestCommand(_alice.Id.Value, "bob@example.com"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be("bob@example.com");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenRequestAlreadyExists()
    {
        var handler = new SendFriendRequestCommandHandler(_userRepository, _friendshipRepository, _unitOfWork);
        await handler.Handle(new SendFriendRequestCommand(_alice.Id.Value, "bob@example.com"), CancellationToken.None);

        var result = await handler.Handle(new SendFriendRequestCommand(_alice.Id.Value, "bob@example.com"), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }
}
