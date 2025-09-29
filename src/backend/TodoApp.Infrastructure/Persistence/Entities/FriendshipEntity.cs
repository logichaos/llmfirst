namespace TodoApp.Infrastructure.Persistence.Entities;

public sealed class FriendshipEntity
{
    public Guid Id { get; set; }
    public Guid RequesterId { get; set; }
    public Guid AddresseeId { get; set; }
    public string Status { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? RespondedAt { get; set; }

    public UserEntity? Requester { get; set; }
    public UserEntity? Addressee { get; set; }
}
