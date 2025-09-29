using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.Friendships;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;

namespace TodoApp.Application.Friendships.Commands.SendFriendRequest;

public sealed record SendFriendRequestCommand(Guid RequesterUserId, string FriendEmail) : IRequest<Result<FriendDto>>;

public sealed class SendFriendRequestCommandHandler : IRequestHandler<SendFriendRequestCommand, Result<FriendDto>>
{
    private readonly UserRepository _userRepository;
    private readonly FriendshipRepository _friendshipRepository;
    private readonly UnitOfWork _unitOfWork;

    public SendFriendRequestCommandHandler(
        UserRepository userRepository,
        FriendshipRepository friendshipRepository,
        UnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _friendshipRepository = friendshipRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FriendDto>> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var requesterId = UserId.FromGuid(request.RequesterUserId);

        var requester = await _userRepository.GetByIdAsync(requesterId, cancellationToken);
        if (requester is null)
        {
            return Result.Fail(ApplicationErrors.Users.NotFound);
        }

        Email email;
        try
        {
            email = Email.Create(request.FriendEmail);
        }
        catch (ArgumentException ex)
        {
            return Result.Fail(new Error(ex.Message));
        }

        var addressee = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (addressee is null)
        {
            return Result.Fail(ApplicationErrors.Users.NotFound);
        }

        if (addressee.Id == requester.Id)
        {
            return Result.Fail(new Error("You cannot friend yourself"));
        }

        var existing = await _friendshipRepository.GetBetweenUsersAsync(requester.Id, addressee.Id, cancellationToken);
        if (existing is not null)
        {
            return Result.Fail(ApplicationErrors.Friendships.AlreadyRequested);
        }

        var friendship = Friendship.Request(requester.Id, addressee.Id);
        requester.LinkFriendship(friendship.Id);
        addressee.LinkFriendship(friendship.Id);

        _friendshipRepository.Add(friendship);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(friendship.ToDto(addressee));
    }
}
