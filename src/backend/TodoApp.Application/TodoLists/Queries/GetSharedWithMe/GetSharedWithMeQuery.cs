using FluentResults;
using MediatR;
using TodoApp.Application.Contracts;
using TodoApp.Application.Mappings;
using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;

namespace TodoApp.Application.TodoLists.Queries.GetSharedWithMe;

public sealed record GetSharedWithMeQuery(Guid UserId) : IRequest<Result<IReadOnlyCollection<TodoListDto>>>;

public sealed class GetSharedWithMeQueryHandler : IRequestHandler<GetSharedWithMeQuery, Result<IReadOnlyCollection<TodoListDto>>>
{
    private readonly TodoListRepository _todoListRepository;

    public GetSharedWithMeQueryHandler(TodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async Task<Result<IReadOnlyCollection<TodoListDto>>> Handle(GetSharedWithMeQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.FromGuid(request.UserId);
        var lists = await _todoListRepository.GetSharedWithUserAsync(userId, cancellationToken);
        var dtos = lists.Select(list => list.ToDto(list.OwnerId == userId)).ToArray();
        return Result.Ok<IReadOnlyCollection<TodoListDto>>(dtos);
    }
}
