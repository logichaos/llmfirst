using FluentAssertions;
using TodoApp.Application.TodoLists.Queries.GetMyTodoLists;
using TodoApp.Application.Tests.Fakes;
using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.Users;
using Xunit;

namespace TodoApp.Application.Tests.TodoLists;

public sealed class GetMyTodoListsQueryTests
{
    private readonly FakeTodoListRepository _todoListRepository = new();
    private readonly User _owner;

    public GetMyTodoListsQueryTests()
    {
        _owner = User.Register(Email.Create("owner@example.com"), "Owner", PasswordHash.FromHash("hash"));
        var list = TodoList.Create(_owner.Id, "Groceries");
        _todoListRepository.Add(list);
    }

    [Fact]
    public async Task Handle_ShouldReturnOwnedLists()
    {
        var handler = new GetMyTodoListsQueryHandler(_todoListRepository);

        var result = await handler.Handle(new GetMyTodoListsQuery(_owner.Id.Value), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }
}
