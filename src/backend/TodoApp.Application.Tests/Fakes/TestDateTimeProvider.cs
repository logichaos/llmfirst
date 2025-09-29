using TodoApp.Application.Abstractions;

namespace TodoApp.Application.Tests.Fakes;

internal sealed class TestDateTimeProvider : DateTimeProvider
{
    public TestDateTimeProvider(DateTimeOffset utcNow)
    {
        UtcNow = utcNow;
    }

    public DateTimeOffset UtcNow { get; }
}
