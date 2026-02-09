using Erp.Application.Abstractions.Persistence;
using Erp.Infrastructure.Persistence;
using Erp.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Erp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Connection string 'SqlServer' is missing.");

        services.AddDbContext<ErpDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName);
                sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "infra");
            }));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();

        return services;
    }
}
