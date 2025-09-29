using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;

namespace TodoApp.Application.TodoLists.Commands.ToggleTodoItem;

public sealed record ToggleTodoItemCommand(Guid ActorUserId, Guid TodoListId, Guid TodoItemId, bool IsCompleted) : IRequest<Result>;

public sealed class ToggleTodoItemCommandHandler : IRequestHandler<ToggleTodoItemCommand, Result>
{
    private readonly TodoListRepository _todoListRepository;
    private readonly UnitOfWork _unitOfWork;

    public ToggleTodoItemCommandHandler(TodoListRepository todoListRepository, UnitOfWork unitOfWork)
    {
        _todoListRepository = todoListRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ToggleTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoListId = TodoListId.FromGuid(request.TodoListId);
        var list = await _todoListRepository.GetByIdAsync(todoListId, cancellationToken);
        if (list is null)
        {
            return Result.Fail(ApplicationErrors.TodoLists.NotFound);
        }

        var actorId = UserId.FromGuid(request.ActorUserId);
        if (!list.CanEdit(actorId))
        {
            return Result.Fail(ApplicationErrors.TodoLists.UnauthorizedAccess);
        }

        try
        {
            list.ToggleItem(TodoItemId.FromGuid(request.TodoItemId), request.IsCompleted);
        }
        catch (DomainException ex)
        {
            return Result.Fail(new Error(ex.Message));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
