namespace TodoApp.Application.Abstractions;

public interface DateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
