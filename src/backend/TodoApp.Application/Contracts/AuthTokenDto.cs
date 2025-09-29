namespace TodoApp.Application.Contracts;

public sealed record AuthTokenDto(string AccessToken, DateTimeOffset ExpiresAt);
