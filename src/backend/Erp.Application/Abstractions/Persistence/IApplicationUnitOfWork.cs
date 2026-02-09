namespace Erp.Application.Abstractions.Persistence;

public interface IApplicationUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
