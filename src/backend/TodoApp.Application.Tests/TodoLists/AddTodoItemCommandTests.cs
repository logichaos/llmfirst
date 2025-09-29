using FluentAssertions;
using TodoApp.Application.TodoLists.Commands.AddTodoItem;
using TodoApp.Application.Tests.Fakes;
using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.Users;
using Xunit;

namespace TodoApp.Application.Tests.TodoLists;

public sealed class AddTodoItemCommandTests
{
    private readonly FakeTodoListRepository _todoListRepository = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly TodoList _todoList;

    public AddTodoItemCommandTests()
    {
        var owner = User.Register(Email.Create("owner@example.com"), "Owner", PasswordHash.FromHash("hash"));
        _todoList = TodoList.Create(owner.Id, "Groceries");
        _todoListRepository.Add(_todoList);
    }

    [Fact]
    public async Task Handle_ShouldAddItem_WhenActorHasPermission()
    {
        var handler = new AddTodoItemCommandHandler(_todoListRepository, _unitOfWork);

        var result = await handler.Handle(new AddTodoItemCommand(_todoList.OwnerId.Value, _todoList.Id.Value, "Buy milk"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Description.Should().Be("Buy milk");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenActorNotAuthorized()
    {
        var handler = new AddTodoItemCommandHandler(_todoListRepository, _unitOfWork);

        var result = await handler.Handle(new AddTodoItemCommand(Guid.NewGuid(), _todoList.Id.Value, "Buy milk"), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }
}
