namespace TodoApp.Infrastructure.Persistence.Entities;

public sealed class UserEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    public ICollection<TodoListEntity> TodoLists { get; set; } = new List<TodoListEntity>();
    public ICollection<FriendshipEntity> FriendshipsRequested { get; set; } = new List<FriendshipEntity>();
    public ICollection<FriendshipEntity> FriendshipsReceived { get; set; } = new List<FriendshipEntity>();
}
