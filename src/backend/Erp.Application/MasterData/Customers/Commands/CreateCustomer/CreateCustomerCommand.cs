using MediatR;

namespace Erp.Application.MasterData.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Codice,
    string RagioneSociale,
    string Email) : IRequest<Guid>;
