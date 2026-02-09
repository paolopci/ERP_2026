using Erp.Application.Abstractions.Persistence;
using Erp.Application.Abstractions.Security;
using Erp.Application.MasterData.Customers.Commands.CreateCustomer;
using FluentAssertions;
using NSubstitute;

namespace Erp.Application.Tests.MasterData;

public sealed class CreateCustomerCommandTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IApplicationUnitOfWork _applicationUnitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly CreateCustomerCommandHandler _sut;

    public CreateCustomerCommandTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _applicationUnitOfWork = Substitute.For<IApplicationUnitOfWork>();
        _tenantContext = Substitute.For<ITenantContext>();
        _sut = new CreateCustomerCommandHandler(_customerRepository, _applicationUnitOfWork, _tenantContext);
    }

    [Fact]
    public async Task Handle_ConTenantValido_RitornaIdNuovoCliente()
    {
        // Arrange
        _tenantContext.CompanyId.Returns(Guid.NewGuid());
        var command = new CreateCustomerCommand("CLI-001", "Cliente Uno", "cliente1@test.local");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_ConTenantValido_SalvaUnitaDiLavoro()
    {
        // Arrange
        _tenantContext.CompanyId.Returns(Guid.NewGuid());
        var command = new CreateCustomerCommand("CLI-001", "Cliente Uno", "cliente1@test.local");

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        await _applicationUnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_SenzaTenant_LanciaEccezione()
    {
        // Arrange
        _tenantContext.CompanyId.Returns((Guid?)null);
        var command = new CreateCustomerCommand("CLI-001", "Cliente Uno", "cliente1@test.local");

        // Act
        var action = async () => await _sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
