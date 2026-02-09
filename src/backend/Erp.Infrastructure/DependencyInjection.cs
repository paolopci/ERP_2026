using Erp.Application.Abstractions.Persistence;
using Erp.Application.Abstractions.Security;
using Erp.Infrastructure.Persistence;
using Erp.Infrastructure.Persistence.Identity;
using Erp.Infrastructure.Persistence.Repositories;
using Erp.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
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

        services.Configure<JwtTokenOptions>(configuration.GetSection(JwtTokenOptions.SectionName));

        services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 10;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ErpDbContext>()
            .AddSignInManager();

        services.AddHttpContextAccessor();

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenIssuer, JwtTokenIssuer>();
        services.AddScoped<ITenantContext, CurrentTenantContext>();

        return services;
    }
}
