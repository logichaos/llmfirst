using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;

namespace TodoApp.Application.Tests.Fakes;

internal sealed class FakeTodoListRepository : TodoListRepository
{
    private readonly Dictionary<TodoListId, TodoList> _todoLists = new();

    public void Add(TodoList todoList)
    {
        _todoLists[todoList.Id] = todoList;
    }

    public Task<TodoList?> GetByIdAsync(TodoListId id, CancellationToken cancellationToken = default)
    {
        _todoLists.TryGetValue(id, out var list);
        return Task.FromResult(list);
    }

    public Task<IReadOnlyList<TodoList>> GetOwnedByUserAsync(UserId ownerId, CancellationToken cancellationToken = default)
    {
        var lists = _todoLists.Values.Where(list => list.OwnerId == ownerId).ToArray();
        return Task.FromResult((IReadOnlyList<TodoList>)lists);
    }

    public Task<IReadOnlyList<TodoList>> GetSharedWithUserAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var lists = _todoLists.Values.Where(list => list.SharedWith.Any(s => s.SharedWith == userId)).ToArray();
        return Task.FromResult((IReadOnlyList<TodoList>)lists);
    }

    public void Update(TodoList todoList)
    {
        _todoLists[todoList.Id] = todoList;
    }

    public void Remove(TodoList todoList)
    {
        _todoLists.Remove(todoList.Id);
    }
}
