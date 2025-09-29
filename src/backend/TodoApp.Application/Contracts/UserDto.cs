namespace TodoApp.Application.Contracts;

public sealed record UserDto(Guid Id, string Email, string DisplayName);
