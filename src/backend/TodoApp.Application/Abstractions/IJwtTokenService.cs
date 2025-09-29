using TodoApp.Application.Contracts;
using TodoApp.Domain.Users;

namespace TodoApp.Application.Abstractions;

public interface JwtTokenService
{
    AuthTokenDto GenerateToken(User user);
}
