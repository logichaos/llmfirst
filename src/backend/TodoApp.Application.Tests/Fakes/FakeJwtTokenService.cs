using TodoApp.Application.Abstractions;
using TodoApp.Application.Contracts;
using TodoApp.Domain.Users;

namespace TodoApp.Application.Tests.Fakes;

internal sealed class FakeJwtTokenService : JwtTokenService
{
    public DateTimeOffset ExpiresAt { get; set; } = new DateTimeOffset(2025, 9, 30, 13, 0, 0, TimeSpan.Zero);

    public AuthTokenDto GenerateToken(User user)
        => new($"TOKEN::{user.Id.Value}", ExpiresAt);
}
