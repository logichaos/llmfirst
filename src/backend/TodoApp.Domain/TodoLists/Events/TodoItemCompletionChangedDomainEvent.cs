using TodoApp.Domain.Shared;

namespace TodoApp.Domain.TodoLists.Events;

public sealed record TodoItemCompletionChangedDomainEvent(TodoListId TodoListId, TodoItemId TodoItemId, bool IsCompleted) : DomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
