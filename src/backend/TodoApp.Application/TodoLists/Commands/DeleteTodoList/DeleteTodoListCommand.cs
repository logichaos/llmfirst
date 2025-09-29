using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.Users;
using TodoApp.Domain.Shared;

namespace TodoApp.Application.TodoLists.Commands.DeleteTodoList;

public sealed record DeleteTodoListCommand(Guid OwnerUserId, Guid TodoListId) : IRequest<Result>;

public sealed class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand, Result>
{
    private readonly TodoListRepository _todoListRepository;
    private readonly UserRepository _userRepository;
    private readonly UnitOfWork _unitOfWork;

    public DeleteTodoListCommandHandler(TodoListRepository todoListRepository, UserRepository userRepository, UnitOfWork unitOfWork)
    {
        _todoListRepository = todoListRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var ownerId = UserId.FromGuid(request.OwnerUserId);
        var todoListId = TodoListId.FromGuid(request.TodoListId);

        var todoList = await _todoListRepository.GetByIdAsync(todoListId, cancellationToken);
        if (todoList is null)
        {
            return Result.Fail(ApplicationErrors.TodoLists.NotFound);
        }

        if (todoList.OwnerId != ownerId)
        {
            return Result.Fail(ApplicationErrors.TodoLists.UnauthorizedAccess);
        }

        var owner = await _userRepository.GetByIdAsync(ownerId, cancellationToken);
        if (owner is not null)
        {
            owner.DetachTodoList(todoList.Id);
        }

        _todoListRepository.Remove(todoList);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
