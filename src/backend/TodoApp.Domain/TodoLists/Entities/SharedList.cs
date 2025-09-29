using TodoApp.Domain.Shared;

namespace TodoApp.Domain.TodoLists.Entities;

public sealed class SharedList : Entity<SharedListId>
{
    private SharedList(SharedListId id, TodoListId todoListId, UserId ownerId, UserId sharedWith, SharePermission permission, DateTimeOffset grantedAt)
    {
        Id = id;
        TodoListId = todoListId;
        OwnerId = ownerId;
        SharedWith = sharedWith;
        Permission = permission;
        GrantedAt = grantedAt;
    }

    public TodoListId TodoListId { get; }
    public UserId OwnerId { get; }
    public UserId SharedWith { get; }
    public SharePermission Permission { get; private set; }
    public DateTimeOffset GrantedAt { get; }

    public static SharedList Create(TodoListId todoListId, UserId ownerId, UserId sharedWith, SharePermission permission)
    {
        if (ownerId == sharedWith)
        {
            throw new DomainException("Owners already have full access to their lists");
        }

        return new SharedList(SharedListId.New(), todoListId, ownerId, sharedWith, permission, DateTimeOffset.UtcNow);
    }

    public void UpdatePermission(SharePermission permission)
    {
        Permission = permission;
    }

    public static SharedList Restore(SharedListId id, TodoListId todoListId, UserId ownerId, UserId sharedWith, SharePermission permission, DateTimeOffset grantedAt)
        => new(id, todoListId, ownerId, sharedWith, permission, grantedAt);
}
