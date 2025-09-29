using TodoApp.Domain.Shared;

namespace TodoApp.Domain.Friendships;

public interface FriendshipRepository
{
    Task<Friendship?> GetByIdAsync(FriendshipId friendshipId, CancellationToken cancellationToken = default);
    Task<Friendship?> GetBetweenUsersAsync(UserId requesterId, UserId addresseeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Friendship>> GetForUserAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<bool> AreFriendsAsync(UserId left, UserId right, CancellationToken cancellationToken = default);
    void Add(Friendship friendship);
    void Update(Friendship friendship);
}
