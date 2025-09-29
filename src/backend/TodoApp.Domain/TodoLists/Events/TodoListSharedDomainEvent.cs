using TodoApp.Domain.Shared;

namespace TodoApp.Domain.TodoLists.Events;

public sealed record TodoListSharedDomainEvent(TodoListId TodoListId, UserId OwnerId, UserId SharedWithUserId, SharePermission Permission) : DomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
