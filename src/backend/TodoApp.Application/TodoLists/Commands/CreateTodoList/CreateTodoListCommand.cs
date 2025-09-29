using FluentResults;
using MediatR;
using TodoApp.Application.Abstractions;
using TodoApp.Application.Common;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.Users;
using TodoApp.Domain.Shared;

namespace TodoApp.Application.TodoLists.Commands.CreateTodoList;

public sealed record CreateTodoListCommand(Guid OwnerUserId, string Title) : IRequest<Result<TodoListDto>>;

public sealed class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, Result<TodoListDto>>
{
    private readonly UserRepository _userRepository;
    private readonly TodoListRepository _todoListRepository;
    private readonly UnitOfWork _unitOfWork;

    public CreateTodoListCommandHandler(UserRepository userRepository, TodoListRepository todoListRepository, UnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _todoListRepository = todoListRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TodoListDto>> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var ownerId = UserId.FromGuid(request.OwnerUserId);
        var owner = await _userRepository.GetByIdAsync(ownerId, cancellationToken);
        if (owner is null)
        {
            return Result.Fail(ApplicationErrors.Users.NotFound);
        }

        TodoList todoList;
        try
        {
            todoList = TodoList.Create(owner.Id, request.Title);
        }
        catch (DomainException ex)
        {
            return Result.Fail(new Error(ex.Message));
        }

        owner.AttachTodoList(todoList.Id);
        _todoListRepository.Add(todoList);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(todoList.ToDto(true));
    }
}
