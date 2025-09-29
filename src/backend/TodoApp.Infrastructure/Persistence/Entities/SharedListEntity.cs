namespace TodoApp.Infrastructure.Persistence.Entities;

public sealed class SharedListEntity
{
    public Guid Id { get; set; }
    public Guid TodoListId { get; set; }
    public Guid OwnerId { get; set; }
    public Guid SharedWithUserId { get; set; }
    public string Permission { get; set; } = default!;
    public DateTimeOffset GrantedAt { get; set; }

    public TodoListEntity? TodoList { get; set; }
}
