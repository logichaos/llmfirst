using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.Shared;

namespace TodoApp.Application.TodoLists.Commands.AddTodoItem;

public sealed record AddTodoItemCommand(Guid ActorUserId, Guid TodoListId, string Description) : IRequest<Result<TodoItemDto>>;

public sealed class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand, Result<TodoItemDto>>
{
    private readonly TodoListRepository _todoListRepository;
    private readonly UnitOfWork _unitOfWork;

    public AddTodoItemCommandHandler(TodoListRepository todoListRepository, UnitOfWork unitOfWork)
    {
        _todoListRepository = todoListRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TodoItemDto>> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
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

        TodoItemDto dto;
        try
        {
            var item = list.AddItem(request.Description);
            dto = item.ToDto();
        }
        catch (DomainException ex)
        {
            return Result.Fail(new Error(ex.Message));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(dto);
    }
}
