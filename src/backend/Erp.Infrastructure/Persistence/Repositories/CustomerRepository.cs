using Erp.Application.Abstractions.Persistence;
using Erp.Application.Common.Models;
using Erp.Domain.MasterData;
using Microsoft.EntityFrameworkCore;

namespace Erp.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly ErpDbContext _dbContext;

    public CustomerRepository(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Cliente cliente, CancellationToken cancellationToken)
    {
        await _dbContext.Clienti.AddAsync(cliente, cancellationToken);
    }

    public Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Clienti
            .Where(x => x.Id == id)
            .Select(x => new CustomerDto(x.Id, x.Codice, x.RagioneSociale, x.Email, x.Attivo))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
