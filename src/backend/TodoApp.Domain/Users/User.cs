using TodoApp.Domain.Shared;
using TodoApp.Domain.Users.Events;

namespace TodoApp.Domain.Users;

public sealed class User : AggregateRoot<UserId>
{
    private readonly HashSet<TodoListId> _ownedTodoLists = new();
    private readonly HashSet<FriendshipId> _friendships = new();

    private User(UserId id, Email email, string displayName, PasswordHash passwordHash)
    {
        Id = id;
        Email = email;
        DisplayName = displayName;
        PasswordHash = passwordHash;
    }

    public Email Email { get; }
    public string DisplayName { get; private set; }
    public PasswordHash PasswordHash { get; private set; }

    public IReadOnlyCollection<TodoListId> OwnedTodoLists => _ownedTodoLists;
    public IReadOnlyCollection<FriendshipId> Friendships => _friendships;

    public static User Register(Email email, string displayName, PasswordHash passwordHash)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new DomainException("Display name cannot be empty");
        }

        var user = new User(UserId.New(), email, displayName.Trim(), passwordHash);
        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id, user.Email));
        return user;
    }

    public void UpdateDisplayName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new DomainException("Display name cannot be empty");
        }

        var normalized = newName.Trim();
        if (normalized.Equals(DisplayName, StringComparison.Ordinal))
        {
            return;
        }

        DisplayName = normalized;
        RaiseDomainEvent(new UserDisplayNameUpdatedDomainEvent(Id, DisplayName));
    }

    public void AttachTodoList(TodoListId todoListId)
    {
        if (todoListId == default)
        {
            throw new ArgumentException("Todo list id cannot be empty", nameof(todoListId));
        }

        _ownedTodoLists.Add(todoListId);
    }

    public void DetachTodoList(TodoListId todoListId)
    {
        if (todoListId == default)
        {
            throw new ArgumentException("Todo list id cannot be empty", nameof(todoListId));
        }

        _ownedTodoLists.Remove(todoListId);
    }

    public void LinkFriendship(FriendshipId friendshipId)
    {
        if (friendshipId == default)
        {
            throw new ArgumentException("Friendship id cannot be empty", nameof(friendshipId));
        }

        _friendships.Add(friendshipId);
    }

    public void UpdatePassword(PasswordHash passwordHash)
    {
        ArgumentNullException.ThrowIfNull(passwordHash);
        PasswordHash = passwordHash;
    }

    public static User Restore(
        UserId id,
        Email email,
        string displayName,
        PasswordHash passwordHash,
        IEnumerable<TodoListId> ownedTodoLists,
        IEnumerable<FriendshipId> friendships)
    {
        var user = new User(id, email, displayName, passwordHash);

        foreach (var listId in ownedTodoLists)
        {
            user._ownedTodoLists.Add(listId);
        }

        foreach (var friendshipId in friendships)
        {
            user._friendships.Add(friendshipId);
        }

        user.ClearDomainEvents();
        return user;
    }
}
