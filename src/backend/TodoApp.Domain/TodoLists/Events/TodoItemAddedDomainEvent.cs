using TodoApp.Domain.Shared;

namespace TodoApp.Domain.TodoLists.Events;

public sealed record TodoItemAddedDomainEvent(TodoListId TodoListId, TodoItemId TodoItemId, string Description) : DomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
