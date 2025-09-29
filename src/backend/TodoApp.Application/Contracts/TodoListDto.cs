namespace TodoApp.Application.Contracts;

public sealed record TodoListDto(
    Guid Id,
    string Title,
    IReadOnlyCollection<TodoItemDto> Items,
    IReadOnlyCollection<SharedListEntryDto> SharedWith,
    bool IsOwner);
