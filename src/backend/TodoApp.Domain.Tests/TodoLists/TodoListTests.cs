using FluentAssertions;
using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.TodoLists.Entities;
using TodoApp.Domain.TodoLists.Events;
using Xunit;

namespace TodoApp.Domain.Tests.TodoLists;

public class TodoListTests
{
    [Fact]
    public void Create_ShouldRaiseEvent()
    {
        var ownerId = UserId.New();

        var list = TodoList.Create(ownerId, "Groceries");

        list.OwnerId.Should().Be(ownerId);
        list.Title.Should().Be("Groceries");
        list.DomainEvents.Should().ContainSingle(e => e is TodoListCreatedDomainEvent);
    }

    [Fact]
    public void AddItem_ShouldCreateNewItem()
    {
        var list = TodoList.Create(UserId.New(), "Groceries");

        var item = list.AddItem("Buy milk");

        list.Items.Should().ContainSingle().Which.Should().Be(item);
        item.Description.Should().Be("Buy milk");
        list.DomainEvents.Should().ContainSingle(e => e is TodoItemAddedDomainEvent);
    }

    [Fact]
    public void ToggleItemCompletion_ShouldUpdateState()
    {
        var list = TodoList.Create(UserId.New(), "Groceries");
        var item = list.AddItem("Buy milk");

        list.ToggleItem(item.Id, true);

        list.Items.Should().ContainSingle().Which.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void ShareWithFriend_ShouldAddShare()
    {
        var ownerId = UserId.New();
        var friendId = UserId.New();
        var list = TodoList.Create(ownerId, "Groceries");

        list.ShareWith(friendId, SharePermission.Editor);

        list.SharedWith.Should().ContainSingle().Which.SharedWith.Should().Be(friendId);
        list.DomainEvents.Should().ContainSingle(e => e is TodoListSharedDomainEvent);
    }
}
