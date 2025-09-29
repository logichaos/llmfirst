using FluentAssertions;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Authentication.Commands.RegisterUser;
using TodoApp.Application.Tests.Fakes;
using Xunit;

namespace TodoApp.Application.Tests.Authentication;

public sealed class RegisterUserCommandTests
{
    private readonly FakeUserRepository _userRepository = new();
    private readonly PasswordHasher _passwordHasher = new FakePasswordHasher();
    private readonly UnitOfWork _unitOfWork = new FakeUnitOfWork();

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenDataValid()
    {
        var handler = new RegisterUserCommandHandler(_userRepository, _passwordHasher, _unitOfWork);
        var command = new RegisterUserCommand("alice@example.com", "Secret123!", "Alice");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be("alice@example.com");
        (await _userRepository.GetByEmailAsync(TodoApp.Domain.Shared.Email.Create("alice@example.com"))).Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEmailAlreadyExists()
    {
        var handler = new RegisterUserCommandHandler(_userRepository, _passwordHasher, _unitOfWork);
        await handler.Handle(new RegisterUserCommand("alice@example.com", "Secret123!", "Alice"), CancellationToken.None);

        var result = await handler.Handle(new RegisterUserCommand("alice@example.com", "Secret123!", "Alice"), CancellationToken.None);

        result.IsFailed.Should().BeTrue();
    }
}
