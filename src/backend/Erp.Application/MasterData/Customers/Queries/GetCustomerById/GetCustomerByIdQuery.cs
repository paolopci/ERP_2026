using Erp.Application.Common.Models;
using MediatR;

namespace Erp.Application.MasterData.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;
