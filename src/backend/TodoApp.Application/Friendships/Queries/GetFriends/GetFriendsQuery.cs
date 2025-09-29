using FluentResults;
using MediatR;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.Friendships;
using TodoApp.Domain.Shared;
using TodoApp.Domain.Users;

namespace TodoApp.Application.Friendships.Queries.GetFriends;

public sealed record GetFriendsQuery(Guid UserId) : IRequest<Result<IReadOnlyCollection<FriendDto>>>;

public sealed class GetFriendsQueryHandler : IRequestHandler<GetFriendsQuery, Result<IReadOnlyCollection<FriendDto>>>
{
    private readonly FriendshipRepository _friendshipRepository;
    private readonly UserRepository _userRepository;

    public GetFriendsQueryHandler(FriendshipRepository friendshipRepository, UserRepository userRepository)
    {
        _friendshipRepository = friendshipRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<IReadOnlyCollection<FriendDto>>> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.FromGuid(request.UserId);
        var friendships = await _friendshipRepository.GetForUserAsync(userId, cancellationToken);
        var friends = new List<FriendDto>(friendships.Count);

        foreach (var friendship in friendships)
        {
            var otherUserId = friendship.RequesterId == userId ? friendship.AddresseeId : friendship.RequesterId;
            var user = await _userRepository.GetByIdAsync(otherUserId, cancellationToken);
            if (user is null)
            {
                continue;
            }

            friends.Add(friendship.ToDto(user));
        }

        return Result.Ok<IReadOnlyCollection<FriendDto>>(friends);
    }
}
