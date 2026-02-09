using Erp.Application.Common.Behaviors;
using Erp.Application.MasterData.Customers.Commands.CreateCustomer;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Erp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}
