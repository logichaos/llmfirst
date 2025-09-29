using FluentAssertions;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;
using TodoApp.Domain.Users.Events;
using Xunit;

namespace TodoApp.Domain.Tests.Users;

public class UserTests
{
    [Fact]
    public void Register_ShouldCreateUserAndRaiseEvent()
    {
        var email = Email.Create("alice@example.com");
        var displayName = "Alice";
        var passwordHash = PasswordHash.FromHash("hashed-value");

        var user = User.Register(email, displayName, passwordHash);

        user.Id.Should().NotBe(UserId.Empty);
        user.Email.Should().Be(email);
        user.DisplayName.Should().Be(displayName);
        user.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserRegisteredDomainEvent>()
            .Which.Email.Should().Be(email);
    }

    [Fact]
    public void UpdateDisplayName_ShouldChangeNameAndRaiseEvent()
    {
        var user = CreateUser();

        user.UpdateDisplayName("New Name");

        user.DisplayName.Should().Be("New Name");
        user.DomainEvents.Should().ContainSingle(e => e is UserDisplayNameUpdatedDomainEvent);
    }

    [Fact]
    public void AttachTodoList_ShouldAddListOnlyOnce()
    {
        var user = CreateUser();
        var listId = TodoListId.New();

        user.AttachTodoList(listId);
        user.AttachTodoList(listId);

        user.OwnedTodoLists.Should().ContainSingle().Which.Should().Be(listId);
    }

    [Fact]
    public void LinkFriendship_ShouldAddWhenNotPresent()
    {
        var user = CreateUser();
        var friendshipId = FriendshipId.New();

        user.LinkFriendship(friendshipId);

        user.Friendships.Should().ContainSingle().Which.Should().Be(friendshipId);
    }

    private static User CreateUser()
    {
        return User.Register(Email.Create("user@example.com"), "User", PasswordHash.FromHash("hash"));
    }
}
