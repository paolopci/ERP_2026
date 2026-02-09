using Erp.Application.Abstractions.Persistence;
using Erp.Application.Abstractions.Security;
using Erp.Domain.MasterData;
using MediatR;

namespace Erp.Application.MasterData.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IApplicationUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IApplicationUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var companyId = _tenantContext.CompanyId ?? throw new InvalidOperationException("Missing tenant company_id claim.");

        var customer = new Cliente(
            Guid.NewGuid(),
            companyId,
            request.Codice.Trim(),
            request.RagioneSociale.Trim(),
            request.Email.Trim(),
            true);

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
