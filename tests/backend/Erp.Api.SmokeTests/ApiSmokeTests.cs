using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Erp.Contracts.Security;
using Erp.Infrastructure.Persistence;
using Erp.Infrastructure.Persistence.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Erp.Api.SmokeTests;

public sealed class ApiSmokeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string TestJwtKey = "TEST_SIGNING_KEY_1234567890_TEST_SIGNING_KEY_1234567890";

    private readonly WebApplicationFactory<Program> _factory;

    public ApiSmokeTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(_ => { });
    }

    [Fact]
    public async Task HealthLive_RitornaOk()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EndpointProtetto_SenzaToken_RitornaUnauthorized()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/inventory/summary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task HealthReady_ConConnessioneNonValida_RitornaServiceUnavailable()
    {
        // Arrange
        using var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ErpDbContext>));
                services.RemoveAll(typeof(ErpDbContext));
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<ErpDbContext>));
                services.AddDbContext<ErpDbContext>(options =>
                    options.UseSqlServer("Server=invalid-host,1433;Database=ErpDb;User Id=sa;Password=invalid;TrustServerCertificate=true;Encrypt=false"));
            });
        });
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task DiagnosticFail_RitornaProblemDetailsConTraceId()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/diagnostic/fail");
        var body = await response.Content.ReadAsStringAsync();

        // Assert
        body.Should().Contain("traceId");
    }

    [Fact]
    public async Task LoginValido_RitornaAccessERefreshToken()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        using var factory = CreateInMemoryFactory();
        await SeedUserAsync(factory.Services, "utente.test", "Password123!", Role.Admin, companyId);
        using var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest("utente.test", "Password123!"));
        var payload = await response.Content.ReadFromJsonAsync<ApiEnvelopeTokenResponse>();

        // Assert
        payload?.Data?.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginNonValido_RitornaUnauthorized()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        using var factory = CreateInMemoryFactory();
        await SeedUserAsync(factory.Services, "utente.test", "Password123!", Role.Admin, companyId);
        using var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest("utente.test", "password-sbagliata"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_RuotaTokenERifiutaRiutilizzoVecchio()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        using var factory = CreateInMemoryFactory();
        await SeedUserAsync(factory.Services, "utente.refresh", "Password123!", Role.Admin, companyId);
        using var client = factory.CreateClient();
        var login = await client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest("utente.refresh", "Password123!"));
        var loginPayload = await login.Content.ReadFromJsonAsync<ApiEnvelopeTokenResponse>();
        var firstRefresh = loginPayload!.Data!.RefreshToken;

        // Act
        var refresh = await client.PostAsJsonAsync("/api/v1/auth/refresh", new RefreshTokenRequest(firstRefresh));
        var oldRefreshReuse = await client.PostAsJsonAsync("/api/v1/auth/refresh", new RefreshTokenRequest(firstRefresh));

        // Assert
        oldRefreshReuse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RuoloSenzaPermesso_RitornaUnauthorized()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        using var factory = CreateInMemoryFactory();
        using var client = factory.CreateClient();
        var token = BuildToken(
            claims: new[]
            {
                new Claim(SecurityClaimTypes.Subject, Guid.NewGuid().ToString()),
                new Claim(SecurityClaimTypes.CompanyId, companyId.ToString()),
                new Claim(SecurityClaimTypes.Role, Role.Commerciale.ToString())
            }
            .Concat(RolePermissions.Matrix[Role.Commerciale].Select(permission => new Claim(SecurityClaimTypes.Permission, permission)))
            .ToArray());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await client.GetAsync("/api/v1/inventory/summary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TokenSenzaCompanyId_RitornaUnauthorized()
    {
        // Arrange
        using var factory = CreateInMemoryFactory();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BuildTokenWithoutCompanyId());

        // Act
        var response = await client.GetAsync("/api/v1/auth/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private WebApplicationFactory<Program> CreateInMemoryFactory()
    {
        var databaseName = $"erp-tests-{Guid.NewGuid()}";
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:SigningKey"] = TestJwtKey,
                    ["Jwt:Issuer"] = "erp-api-tests",
                    ["Jwt:Audience"] = "erp-web-tests"
                });
            });
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ErpDbContext>));
                services.RemoveAll(typeof(ErpDbContext));
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<ErpDbContext>));

                var sqlServerRegistrations = services
                    .Where(d =>
                        (d.ServiceType.Namespace?.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (d.ImplementationType?.Namespace?.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();

                foreach (var registration in sqlServerRegistrations)
                {
                    services.Remove(registration);
                }

                services.AddDbContext<ErpDbContext>(options => options.UseInMemoryDatabase(databaseName));
            });
        });
    }

    private static async Task SeedUserAsync(IServiceProvider services, string username, string password, Role role, Guid companyId)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        if (!await roleManager.RoleExistsAsync(role.ToString()))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(role.ToString()));
        }

        var existing = await userManager.FindByNameAsync(username);
        if (existing is not null)
        {
            return;
        }

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Email = $"{username}@local.test",
            CompanyId = companyId,
            EmailConfirmed = true,
            IsActive = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Cannot seed auth user for tests.");
        }

        await userManager.AddToRoleAsync(user, role.ToString());
    }

    private static async Task<string> LoginAndGetAccessTokenAsync(HttpClient client, string username, string password)
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest(username, password));
        var payload = await response.Content.ReadFromJsonAsync<ApiEnvelopeTokenResponse>();
        return payload!.Data!.AccessToken;
    }

    private static string BuildTokenWithoutCompanyId()
    {
        return BuildToken(
            [
                new Claim(SecurityClaimTypes.Subject, Guid.NewGuid().ToString()),
                new Claim(SecurityClaimTypes.Role, Role.Admin.ToString()),
                new Claim(SecurityClaimTypes.Permission, PermissionCatalog.Administration)
            ]);
    }

    private static string BuildToken(IReadOnlyCollection<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestJwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "erp-api-tests",
            audience: "erp-web-tests",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private sealed record ApiEnvelopeTokenResponse(TokenResponse? Data);
}
