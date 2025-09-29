namespace TodoApp.Application.Abstractions;

public interface UnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
