using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;

namespace TodoApp.Application.Authentication.Commands.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : IRequest<Result<AuthenticationResultDto>>;

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthenticationResultDto>>
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly JwtTokenService _jwtTokenService;

    public LoginUserCommandHandler(
        UserRepository userRepository,
        PasswordHasher passwordHasher,
        JwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthenticationResultDto>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        Email email;
        try
        {
            email = Email.Create(command.Email);
        }
        catch (ArgumentException)
        {
            return Result.Fail(ApplicationErrors.Users.InvalidCredentials);
        }

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user is null)
        {
            return Result.Fail(ApplicationErrors.Users.InvalidCredentials);
        }

        if (!_passwordHasher.Verify(command.Password, user.PasswordHash.Value))
        {
            return Result.Fail(ApplicationErrors.Users.InvalidCredentials);
        }

        var token = _jwtTokenService.GenerateToken(user);
        var result = new AuthenticationResultDto(token, user.ToDto());

        return Result.Ok(result);
    }
}
