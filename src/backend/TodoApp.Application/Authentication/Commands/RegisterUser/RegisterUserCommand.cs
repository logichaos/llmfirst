using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;

namespace TodoApp.Application.Authentication.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password, string DisplayName) : IRequest<Result<UserDto>>;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserDto>>
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly UnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(UserRepository userRepository, PasswordHasher passwordHasher, UnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        Email email;
        try
        {
            email = Email.Create(command.Email);
        }
        catch (ArgumentException ex)
        {
            return Result.Fail(new Error(ex.Message));
        }

        if (await _userRepository.EmailExistsAsync(email, cancellationToken))
        {
            return Result.Fail(ApplicationErrors.Users.EmailAlreadyTaken(email.Value));
        }

        var hashed = _passwordHasher.Hash(command.Password);
        var user = User.Register(email, command.DisplayName, PasswordHash.FromHash(hashed));

        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(user.ToDto());
    }
}
