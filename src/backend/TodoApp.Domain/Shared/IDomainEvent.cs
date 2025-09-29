namespace TodoApp.Domain.Shared;

public interface DomainEvent
{
    DateTimeOffset OccurredOn { get; }
}
