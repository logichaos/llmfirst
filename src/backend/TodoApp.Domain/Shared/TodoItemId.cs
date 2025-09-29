namespace TodoApp.Domain.Shared;

public readonly record struct TodoItemId(Guid Value)
{
    public static TodoItemId New() => new(Guid.NewGuid());

    public static TodoItemId FromGuid(Guid value)
        => value == Guid.Empty ? throw new ArgumentException("TodoItemId cannot be empty", nameof(value)) : new TodoItemId(value);

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(TodoItemId id) => id.Value;
}
