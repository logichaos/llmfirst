namespace TodoApp.Application.Contracts;

public sealed record TodoItemDto(Guid Id, string Description, bool IsCompleted);
