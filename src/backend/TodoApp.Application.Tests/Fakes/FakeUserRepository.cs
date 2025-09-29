using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;

namespace TodoApp.Application.Tests.Fakes;

internal sealed class FakeUserRepository : UserRepository
{
    private readonly Dictionary<UserId, User> _users = new();

    public void Add(User user)
    {
        _users[user.Id] = user;
    }

    public Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default)
        => Task.FromResult(_users.Values.Any(u => u.Email == email));

    public Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        => Task.FromResult(_users.Values.SingleOrDefault(u => u.Email == email));

    public Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        _users.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }
}
