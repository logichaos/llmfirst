using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;
using TodoApp.Infrastructure.Persistence.Entities;

namespace TodoApp.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository : Domain.Users.UserRepository
{
    private readonly TodoAppDbContext _dbContext;

    public UserRepository(TodoAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(User user)
    {
        var entity = new UserEntity
        {
            Id = user.Id.Value,
            Email = user.Email.Value,
            DisplayName = user.DisplayName,
            PasswordHash = user.PasswordHash.Value
        };

        _dbContext.Users.Add(entity);
    }

    public async Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default)
        => await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Email == email.Value, cancellationToken);

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email.Value, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        return await RehydrateAsync(entity, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id.Value, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        return await RehydrateAsync(entity, cancellationToken);
    }

    private async Task<User> RehydrateAsync(UserEntity entity, CancellationToken cancellationToken)
    {
        var ownedListIds = await _dbContext.TodoLists.AsNoTracking()
            .Where(t => t.OwnerId == entity.Id)
            .Select(t => TodoListId.FromGuid(t.Id))
            .ToListAsync(cancellationToken);

        var friendshipIds = await _dbContext.Friendships.AsNoTracking()
            .Where(f => f.RequesterId == entity.Id || f.AddresseeId == entity.Id)
            .Select(f => FriendshipId.FromGuid(f.Id))
            .ToListAsync(cancellationToken);

        return User.Restore(
            UserId.FromGuid(entity.Id),
            Email.Create(entity.Email),
            entity.DisplayName,
            PasswordHash.FromHash(entity.PasswordHash),
            ownedListIds,
            friendshipIds);
    }
}
