using Erp.Application.Common.Models;
using Erp.Domain.MasterData;

namespace Erp.Application.Abstractions.Persistence;

public interface ICustomerRepository
{
    Task AddAsync(Cliente cliente, CancellationToken cancellationToken);

    Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
