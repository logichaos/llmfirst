using FluentAssertions;
using TodoApp.Application.TodoLists.Commands.ShareTodoList;
using TodoApp.Application.Tests.Fakes;
using TodoApp.Domain.Friendships;
using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.Users;
using Xunit;

namespace TodoApp.Application.Tests.TodoLists;

public sealed class ShareTodoListCommandTests
{
    private readonly FakeTodoListRepository _todoListRepository = new();
    private readonly FakeFriendshipRepository _friendshipRepository = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly User _owner;
    private readonly User _friend;
    private readonly TodoList _todoList;

    public ShareTodoListCommandTests()
    {
        _owner = User.Register(Email.Create("owner@example.com"), "Owner", PasswordHash.FromHash("hash"));
        _friend = User.Register(Email.Create("friend@example.com"), "Friend", PasswordHash.FromHash("hash"));

        _todoList = TodoList.Create(_owner.Id, "Groceries");
        _todoListRepository.Add(_todoList);

        var friendship = Friendship.Request(_owner.Id, _friend.Id);
        friendship.Accept(_friend.Id);
        _friendshipRepository.Add(friendship);
    }

    [Fact]
    public async Task Handle_ShouldShareList_WhenFriendshipAccepted()
    {
        var handler = new ShareTodoListCommandHandler(_todoListRepository, _friendshipRepository, _unitOfWork);

        var result = await handler.Handle(new ShareTodoListCommand(_owner.Id.Value, _todoList.Id.Value, _friend.Id.Value, SharePermission.Editor), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.SharedWith.Should().ContainSingle(entry => entry.SharedWithUserId == _friend.Id.Value);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenUsersNotFriends()
    {
        var handler = new ShareTodoListCommandHandler(_todoListRepository, new FakeFriendshipRepository(), _unitOfWork);

        var result = await handler.Handle(new ShareTodoListCommand(_owner.Id.Value, _todoList.Id.Value, _friend.Id.Value, SharePermission.Editor), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }
}
