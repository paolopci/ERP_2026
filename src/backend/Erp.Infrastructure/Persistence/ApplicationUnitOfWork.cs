using Erp.Application.Abstractions.Persistence;

namespace Erp.Infrastructure.Persistence;

public sealed class ApplicationUnitOfWork : IApplicationUnitOfWork
{
    private readonly ErpDbContext _dbContext;

    public ApplicationUnitOfWork(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
