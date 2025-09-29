namespace TodoApp.Application.Contracts;

public sealed record AuthenticationResultDto(AuthTokenDto Token, UserDto User);
