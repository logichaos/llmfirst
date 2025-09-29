using TodoApp.Domain.Shared;

namespace TodoApp.Domain.TodoLists;

public interface TodoListRepository
{
    Task<TodoList?> GetByIdAsync(TodoListId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoList>> GetOwnedByUserAsync(UserId ownerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoList>> GetSharedWithUserAsync(UserId userId, CancellationToken cancellationToken = default);
    void Add(TodoList todoList);
    void Update(TodoList todoList);
    void Remove(TodoList todoList);
}
