using FluentResults;
using MediatR;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.Shared;

namespace TodoApp.Application.TodoLists.Queries.GetMyTodoLists;

public sealed record GetMyTodoListsQuery(Guid UserId) : IRequest<Result<IReadOnlyCollection<TodoListDto>>>;

public sealed class GetMyTodoListsQueryHandler : IRequestHandler<GetMyTodoListsQuery, Result<IReadOnlyCollection<TodoListDto>>>
{
    private readonly TodoListRepository _todoListRepository;

    public GetMyTodoListsQueryHandler(TodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<IReadOnlyCollection<TodoListDto>>> Handle(GetMyTodoListsQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.FromGuid(request.UserId);
        var lists = await _todoListRepository.GetOwnedByUserAsync(userId, cancellationToken);
        var dtos = lists.Select(list => list.ToDto(true)).ToArray();
        return Result.Ok<IReadOnlyCollection<TodoListDto>>(dtos);
    }
}
