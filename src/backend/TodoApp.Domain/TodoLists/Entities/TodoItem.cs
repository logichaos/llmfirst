using TodoApp.Domain.Shared;

namespace TodoApp.Domain.TodoLists.Entities;

public sealed class TodoItem : Entity<TodoItemId>
{
    private TodoItem(TodoItemId id, string description)
    {
        Id = id;
        Description = description;
    }

    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }

    public static TodoItem Create(TodoItemId id, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Todo item description cannot be empty");
        }

        return new TodoItem(id, description.Trim());
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Todo item description cannot be empty");
        }

        Description = description.Trim();
    }

    public void SetCompletion(bool isCompleted)
    {
        IsCompleted = isCompleted;
    }

    public static TodoItem Restore(TodoItemId id, string description, bool isCompleted)
    {
        var item = new TodoItem(id, description)
        {
            IsCompleted = isCompleted
        };

        return item;
    }
}
