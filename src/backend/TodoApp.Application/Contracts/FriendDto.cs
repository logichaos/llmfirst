namespace TodoApp.Application.Contracts;

public sealed record FriendDto(Guid FriendshipId, Guid UserId, string Email, string DisplayName, string Status);
