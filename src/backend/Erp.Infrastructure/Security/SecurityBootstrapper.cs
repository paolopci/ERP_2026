using Erp.Contracts.Security;
using Erp.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erp.Infrastructure.Security;

public static class SecurityBootstrapper
{
    public static async Task SeedIdentityAsync(this IServiceProvider services, IConfiguration configuration, IHostEnvironment environment)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SecurityBootstrapper");

        try
        {
            foreach (var role in Enum.GetNames<Role>())
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Identity role seed skipped because persistence is not reachable.");
            return;
        }

        if (!environment.IsDevelopment() || !configuration.GetValue("SecurityBootstrap:SeedAdmin", false))
        {
            return;
        }

        var adminUsername = configuration["SecurityBootstrap:AdminUsername"];
        var adminPassword = configuration["SecurityBootstrap:AdminPassword"];
        var adminCompanyIdRaw = configuration["SecurityBootstrap:AdminCompanyId"];

        if (string.IsNullOrWhiteSpace(adminUsername) || string.IsNullOrWhiteSpace(adminPassword) || !Guid.TryParse(adminCompanyIdRaw, out var adminCompanyId))
        {
            logger.LogWarning("Admin bootstrap skipped: missing SecurityBootstrap settings.");
            return;
        }

        var existing = await userManager.FindByNameAsync(adminUsername);
        if (existing is not null)
        {
            return;
        }

        var adminUser = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = adminUsername,
            Email = $"{adminUsername}@local.test",
            CompanyId = adminCompanyId,
            EmailConfirmed = true,
            IsActive = true
        };

        var createResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(x => x.Description));
            logger.LogError("Admin bootstrap failed: {Errors}", errors);
            return;
        }

        await userManager.AddToRoleAsync(adminUser, Role.Admin.ToString());
    }
}
