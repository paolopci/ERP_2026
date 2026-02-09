using Erp.Application.Abstractions.Persistence;
using Erp.Application.Common.Models;
using Erp.Application.MasterData.Customers.Commands.CreateCustomer;
using Erp.Domain.MasterData;

namespace Erp.Application.Tests.MasterData;

public sealed class CreateCustomerCommandTests
{
    [Fact]
    public async Task Handle_ValidRequest_ReturnsNewCustomerId()
    {
        var repository = new InMemoryCustomerRepository();
        var unitOfWork = new InMemoryUnitOfWork();
        var handler = new CreateCustomerCommandHandler(repository, unitOfWork);

        var result = await handler.Handle(
            new CreateCustomerCommand("CLI-001", "Cliente Uno", "cliente1@test.local"),
            CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result);
        Assert.Single(repository.Items);
        Assert.Equal(1, unitOfWork.SaveCalls);
    }

    [Fact]
    public void Validator_InvalidEmail_ReturnsValidationError()
    {
        var validator = new CreateCustomerCommandValidator();

        var result = validator.Validate(new CreateCustomerCommand("CLI-001", "Cliente Uno", "mail_non_valida"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    private sealed class InMemoryCustomerRepository : ICustomerRepository
    {
        public List<Cliente> Items { get; } = [];

        public Task AddAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            Items.Add(cliente);
            return Task.CompletedTask;
        }

        public Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult<CustomerDto?>(null);
        }
    }

    private sealed class InMemoryUnitOfWork : IApplicationUnitOfWork
    {
        public int SaveCalls { get; private set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SaveCalls++;
            return Task.FromResult(1);
        }
    }
}
