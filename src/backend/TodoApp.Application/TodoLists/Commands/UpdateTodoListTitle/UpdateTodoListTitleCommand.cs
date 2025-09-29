using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.Shared;

namespace TodoApp.Application.TodoLists.Commands.UpdateTodoListTitle;

public sealed record UpdateTodoListTitleCommand(Guid OwnerUserId, Guid TodoListId, string Title) : IRequest<Result<TodoListDto>>;

public sealed class UpdateTodoListTitleCommandHandler : IRequestHandler<UpdateTodoListTitleCommand, Result<TodoListDto>>
{
    private readonly TodoListRepository _todoListRepository;
    private readonly UnitOfWork _unitOfWork;

    public UpdateTodoListTitleCommandHandler(TodoListRepository todoListRepository, UnitOfWork unitOfWork)
    {
        _todoListRepository = todoListRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TodoListDto>> Handle(UpdateTodoListTitleCommand request, CancellationToken cancellationToken)
    {
        var todoListId = TodoListId.FromGuid(request.TodoListId);
        var list = await _todoListRepository.GetByIdAsync(todoListId, cancellationToken);
        if (list is null)
        {
            return Result.Fail(ApplicationErrors.TodoLists.NotFound);
        }

        var ownerId = UserId.FromGuid(request.OwnerUserId);
        if (list.OwnerId != ownerId)
        {
            return Result.Fail(ApplicationErrors.TodoLists.UnauthorizedAccess);
        }

        try
        {
            list.Rename(request.Title);
        }
        catch (DomainException ex)
        {
            return Result.Fail(new Error(ex.Message));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(list.ToDto(true));
    }
}
