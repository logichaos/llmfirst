using TodoApp.Application.Abstractions;

namespace TodoApp.Application.Tests.Fakes;

internal sealed class FakeUnitOfWork : UnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(1);
}
