using TodoApp.Domain.Shared;

namespace TodoApp.Domain.Users.Events;

public sealed record UserDisplayNameUpdatedDomainEvent(UserId UserId, string DisplayName) : DomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
