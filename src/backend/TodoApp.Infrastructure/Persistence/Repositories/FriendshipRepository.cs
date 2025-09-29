using Microsoft.EntityFrameworkCore;

using TodoApp.Domain.Friendships;
using TodoApp.Domain.Shared;
using TodoApp.Infrastructure.Persistence.Entities;

namespace TodoApp.Infrastructure.Persistence.Repositories;

internal sealed class FriendshipRepository : Domain.Friendships.FriendshipRepository {
  private readonly TodoAppDbContext _dbContext;

  public FriendshipRepository(TodoAppDbContext dbContext) {
    _dbContext = dbContext;
  }

  public void Add(Friendship friendship) {
    var entity = new FriendshipEntity {
      Id = friendship.Id.Value,
      RequesterId = friendship.RequesterId.Value,
      AddresseeId = friendship.AddresseeId.Value,
      Status = friendship.Status.ToString(),
      CreatedAt = friendship.CreatedAt,
      RespondedAt = friendship.RespondedAt
    };

    _dbContext.Friendships.Add(entity);
  }

  public async Task<bool> AreFriendsAsync(UserId left, UserId right, CancellationToken cancellationToken = default) {
    var accepted = FriendshipStatus.Accepted.ToString();
    return await _dbContext.Friendships.AsNoTracking().AnyAsync(f =>
        f.Status == accepted &&
        ((f.RequesterId == left.Value && f.AddresseeId == right.Value) ||
         (f.RequesterId == right.Value && f.AddresseeId == left.Value)), cancellationToken);
  }

  public async Task<Friendship?> GetBetweenUsersAsync(UserId requesterId, UserId addresseeId, CancellationToken cancellationToken = default) {
    var entity = await _dbContext.Friendships.AsNoTracking()
        .FirstOrDefaultAsync(f => f.RequesterId == requesterId.Value && f.AddresseeId == addresseeId.Value, cancellationToken);

    return entity is null ? null : Rehydrate(entity);
  }

  public async Task<IReadOnlyList<Friendship>> GetForUserAsync(UserId userId, CancellationToken cancellationToken = default) {
    var entities = await _dbContext.Friendships.AsNoTracking()
        .Where(f => f.RequesterId == userId.Value || f.AddresseeId == userId.Value)
        .ToListAsync(cancellationToken);

    return entities.Select(Rehydrate).ToArray();
  }

  public async Task<Friendship?> GetByIdAsync(FriendshipId friendshipId, CancellationToken cancellationToken = default) {
    var entity = await _dbContext.Friendships.AsNoTracking()
        .FirstOrDefaultAsync(f => f.Id == friendshipId.Value, cancellationToken);

    return entity is null ? null : Rehydrate(entity);
  }

  private static Friendship Rehydrate(FriendshipEntity entity) {
    var status = Enum.Parse<FriendshipStatus>(entity.Status);
    return Friendship.Restore(
        FriendshipId.FromGuid(entity.Id),
        UserId.FromGuid(entity.RequesterId),
        UserId.FromGuid(entity.AddresseeId),
        status,
        entity.CreatedAt,
        entity.RespondedAt);
  }

  public void Update(Friendship friendship) {
    var entity = new FriendshipEntity {
      Id = friendship.Id.Value,
      RequesterId = friendship.RequesterId.Value,
      AddresseeId = friendship.AddresseeId.Value,
      Status = friendship.Status.ToString(),
      CreatedAt = friendship.CreatedAt,
      RespondedAt = friendship.RespondedAt
    };

    _dbContext.Friendships.Update(entity);
  }
}
