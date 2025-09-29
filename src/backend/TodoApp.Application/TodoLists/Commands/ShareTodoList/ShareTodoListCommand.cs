using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.Friendships;
using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;

namespace TodoApp.Application.TodoLists.Commands.ShareTodoList;

public sealed record ShareTodoListCommand(Guid OwnerUserId, Guid TodoListId, Guid FriendUserId, SharePermission Permission) : IRequest<Result<TodoListDto>>;

public sealed class ShareTodoListCommandHandler : IRequestHandler<ShareTodoListCommand, Result<TodoListDto>>
{
    private readonly TodoListRepository _todoListRepository;
    private readonly FriendshipRepository _friendshipRepository;
    private readonly UnitOfWork _unitOfWork;

    public ShareTodoListCommandHandler(
        TodoListRepository todoListRepository,
        FriendshipRepository friendshipRepository,
        UnitOfWork unitOfWork)
    {
        _todoListRepository = todoListRepository;
        _friendshipRepository = friendshipRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TodoListDto>> Handle(ShareTodoListCommand request, CancellationToken cancellationToken)
    {
        var ownerId = UserId.FromGuid(request.OwnerUserId);
        var todoListId = TodoListId.FromGuid(request.TodoListId);
        var friendId = UserId.FromGuid(request.FriendUserId);

        var todoList = await _todoListRepository.GetByIdAsync(todoListId, cancellationToken);
        if (todoList is null)
        {
            return Result.Fail(ApplicationErrors.TodoLists.NotFound);
        }

        if (todoList.OwnerId != ownerId)
        {
            return Result.Fail(ApplicationErrors.TodoLists.UnauthorizedAccess);
        }

        var areFriends = await _friendshipRepository.AreFriendsAsync(ownerId, friendId, cancellationToken);
        if (!areFriends)
        {
            return Result.Fail(ApplicationErrors.Friendships.NotAuthorized);
        }

        todoList.ShareWith(friendId, request.Permission);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(todoList.ToDto(true));
    }
}
