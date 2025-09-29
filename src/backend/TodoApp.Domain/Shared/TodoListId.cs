namespace TodoApp.Domain.Shared;

public readonly record struct TodoListId(Guid Value)
{
    public static TodoListId New() => new(Guid.NewGuid());

    public static TodoListId FromGuid(Guid value)
        => value == Guid.Empty ? throw new ArgumentException("TodoListId cannot be empty", nameof(value)) : new TodoListId(value);

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(TodoListId id) => id.Value;
}
