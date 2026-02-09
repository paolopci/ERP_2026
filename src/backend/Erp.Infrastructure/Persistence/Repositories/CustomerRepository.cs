using Erp.Application.Abstractions.Persistence;
using Erp.Application.Abstractions.Security;
using Erp.Application.Common.Models;
using Erp.Domain.MasterData;
using Microsoft.EntityFrameworkCore;

namespace Erp.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly ErpDbContext _dbContext;
    private readonly ITenantContext _tenantContext;

    public CustomerRepository(ErpDbContext dbContext, ITenantContext tenantContext)
    {
        _dbContext = dbContext;
        _tenantContext = tenantContext;
    }

    public async Task AddAsync(Cliente cliente, CancellationToken cancellationToken)
    {
        await _dbContext.Clienti.AddAsync(cliente, cancellationToken);
    }

    public Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var companyId = _tenantContext.CompanyId ?? Guid.Empty;

        return _dbContext.Clienti
            .Where(x => x.Id == id && x.CompanyId == companyId)
            .Select(x => new CustomerDto(x.Id, x.Codice, x.RagioneSociale, x.Email, x.Attivo))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
