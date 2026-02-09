using FluentValidation;

namespace Erp.Application.MasterData.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Codice)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.RagioneSociale)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150);
    }
}
