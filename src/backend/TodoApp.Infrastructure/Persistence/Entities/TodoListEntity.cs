namespace TodoApp.Infrastructure.Persistence.Entities;

public sealed class TodoListEntity
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Title { get; set; } = default!;

    public UserEntity? Owner { get; set; }
    public ICollection<TodoItemEntity> Items { get; set; } = new List<TodoItemEntity>();
    public ICollection<SharedListEntity> SharedWith { get; set; } = new List<SharedListEntity>();
}
