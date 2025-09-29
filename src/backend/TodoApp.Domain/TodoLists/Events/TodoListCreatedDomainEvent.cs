using TodoApp.Domain.Shared;

namespace TodoApp.Domain.TodoLists.Events;

public sealed record TodoListCreatedDomainEvent(TodoListId TodoListId, UserId OwnerId, string Title) : DomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
