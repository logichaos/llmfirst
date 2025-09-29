namespace TodoApp.Infrastructure.Persistence.Entities;

public sealed class TodoItemEntity
{
    public Guid Id { get; set; }
    public Guid TodoListId { get; set; }
    public string Description { get; set; } = default!;
    public bool IsCompleted { get; set; }

    public TodoListEntity? TodoList { get; set; }
}
