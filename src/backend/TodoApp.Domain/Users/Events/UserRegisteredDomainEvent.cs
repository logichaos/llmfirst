using TodoApp.Domain.Shared;

namespace TodoApp.Domain.Users.Events;

public sealed record UserRegisteredDomainEvent(UserId UserId, Email Email) : DomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
