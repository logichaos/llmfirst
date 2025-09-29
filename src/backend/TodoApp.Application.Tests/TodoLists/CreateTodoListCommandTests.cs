using FluentAssertions;
using TodoApp.Application.TodoLists.Commands.CreateTodoList;
using TodoApp.Application.Tests.Fakes;
using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.Users;
using Xunit;

namespace TodoApp.Application.Tests.TodoLists;

public sealed class CreateTodoListCommandTests
{
    private readonly FakeUserRepository _userRepository = new();
    private readonly FakeTodoListRepository _todoListRepository = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly User _owner;

    public CreateTodoListCommandTests()
    {
        _owner = User.Register(Email.Create("owner@example.com"), "Owner", PasswordHash.FromHash("hash"));
        _userRepository.Add(_owner);
    }

    [Fact]
    public async Task Handle_ShouldCreateTodoList_ForOwner()
    {
        var handler = new CreateTodoListCommandHandler(_userRepository, _todoListRepository, _unitOfWork);

        var result = await handler.Handle(new CreateTodoListCommand(_owner.Id.Value, "Groceries"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Groceries");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenOwnerMissing()
    {
        var handler = new CreateTodoListCommandHandler(_userRepository, _todoListRepository, _unitOfWork);

        var result = await handler.Handle(new CreateTodoListCommand(Guid.NewGuid(), "Groceries"), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }
}
