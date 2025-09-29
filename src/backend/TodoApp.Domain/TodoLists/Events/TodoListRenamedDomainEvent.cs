using TodoApp.Domain.Shared;

namespace TodoApp.Domain.TodoLists.Events;

public sealed record TodoListRenamedDomainEvent(TodoListId TodoListId, string Title) : DomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
