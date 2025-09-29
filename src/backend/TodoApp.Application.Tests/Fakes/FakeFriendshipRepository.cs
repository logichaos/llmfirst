using TodoApp.Domain.Friendships;
using TodoApp.Domain.Shared;

namespace TodoApp.Application.Tests.Fakes;

internal sealed class FakeFriendshipRepository : FriendshipRepository
{
    private readonly Dictionary<FriendshipId, Friendship> _friendships = new();

    public void Add(Friendship friendship)
    {
        _friendships[friendship.Id] = friendship;
    }

    public void Update(Friendship friendship)
    {
        _friendships[friendship.Id] = friendship;
    }

    public Task<bool> AreFriendsAsync(UserId left, UserId right, CancellationToken cancellationToken = default)
    {
        var result = _friendships.Values.Any(f =>
            f.Status == FriendshipStatus.Accepted &&
            ((f.RequesterId == left && f.AddresseeId == right) || (f.RequesterId == right && f.AddresseeId == left)));

        return Task.FromResult(result);
    }

    public Task<Friendship?> GetBetweenUsersAsync(UserId requesterId, UserId addresseeId, CancellationToken cancellationToken = default)
    {
        var friendship = _friendships.Values.SingleOrDefault(f =>
            f.RequesterId == requesterId && f.AddresseeId == addresseeId);
        return Task.FromResult(friendship);
    }

    public Task<IReadOnlyList<Friendship>> GetForUserAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var friendships = _friendships.Values.Where(f => f.RequesterId == userId || f.AddresseeId == userId).ToArray();
        return Task.FromResult((IReadOnlyList<Friendship>)friendships);
    }

    public Task<Friendship?> GetByIdAsync(FriendshipId friendshipId, CancellationToken cancellationToken = default)
    {
        _friendships.TryGetValue(friendshipId, out var friendship);
        return Task.FromResult(friendship);
    }
}
