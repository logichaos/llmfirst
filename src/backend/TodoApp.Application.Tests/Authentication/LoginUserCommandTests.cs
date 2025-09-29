using FluentAssertions;
using TodoApp.Application.Authentication.Commands.LoginUser;
using TodoApp.Application.Authentication.Commands.RegisterUser;
using TodoApp.Application.Contracts;
using TodoApp.Application.Tests.Fakes;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;
using Xunit;

namespace TodoApp.Application.Tests.Authentication;

public sealed class LoginUserCommandTests
{
    private readonly FakeUserRepository _userRepository = new();
    private readonly FakePasswordHasher _passwordHasher = new();
    private readonly FakeJwtTokenService _jwtTokenService = new();

    public LoginUserCommandTests()
    {
        var passwordHash = _passwordHasher.Hash("Secret123!");
        var user = User.Register(Email.Create("alice@example.com"), "Alice", PasswordHash.FromHash(passwordHash));
        _userRepository.Add(user);
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsValid()
    {
    var handler = new LoginUserCommandHandler(_userRepository, _passwordHasher, _jwtTokenService);

        var result = await handler.Handle(new LoginUserCommand("alice@example.com", "Secret123!"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    result.Value.Token.Should().BeOfType<AuthTokenDto>();
        result.Value.Token.AccessToken.Should().StartWith("TOKEN::");
    result.Value.Token.ExpiresAt.Should().Be(_jwtTokenService.ExpiresAt);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenPasswordInvalid()
    {
    var handler = new LoginUserCommandHandler(_userRepository, _passwordHasher, _jwtTokenService);

        var result = await handler.Handle(new LoginUserCommand("alice@example.com", "wrong"), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }
}
