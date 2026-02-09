using Erp.Application.Abstractions.Persistence;
using Erp.Application.Common.Models;
using MediatR;

namespace Erp.Application.MasterData.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return _customerRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
