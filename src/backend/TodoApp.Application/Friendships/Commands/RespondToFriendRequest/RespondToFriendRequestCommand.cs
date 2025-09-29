using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.Friendships;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;

namespace TodoApp.Application.Friendships.Commands.RespondToFriendRequest;

public sealed record RespondToFriendRequestCommand(Guid FriendshipId, Guid ResponderUserId, bool Accept) : IRequest<Result<FriendDto>>;

public sealed class RespondToFriendRequestCommandHandler : IRequestHandler<RespondToFriendRequestCommand, Result<FriendDto>>
{
    private readonly FriendshipRepository _friendshipRepository;
    private readonly UserRepository _userRepository;
    private readonly UnitOfWork _unitOfWork;

    public RespondToFriendRequestCommandHandler(
        FriendshipRepository friendshipRepository,
        UserRepository userRepository,
        UnitOfWork unitOfWork)
    {
        _friendshipRepository = friendshipRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FriendDto>> Handle(RespondToFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var friendshipId = FriendshipId.FromGuid(request.FriendshipId);
        var responderId = UserId.FromGuid(request.ResponderUserId);

        var friendship = await _friendshipRepository.GetByIdAsync(friendshipId, cancellationToken);
        if (friendship is null)
        {
            return Result.Fail(ApplicationErrors.Friendships.NotFound);
        }

        if (!friendship.AddresseeId.Equals(responderId))
        {
            return Result.Fail(ApplicationErrors.Friendships.NotAuthorized);
        }

        if (request.Accept)
        {
            friendship.Accept(responderId);
        }
        else
        {
            friendship.Reject(responderId);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var otherUserId = friendship.RequesterId;
        var friend = await _userRepository.GetByIdAsync(otherUserId, cancellationToken)
            ?? throw new InvalidOperationException("Requester user must exist");

        return Result.Ok(friendship.ToDto(friend));
    }
}
