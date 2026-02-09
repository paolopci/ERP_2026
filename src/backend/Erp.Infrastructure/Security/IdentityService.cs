using System.Security.Claims;
using Erp.Application.Abstractions.Security;
using Erp.Application.Common.Security;
using Erp.Contracts.Security;
using Erp.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Erp.Infrastructure.Security;

public sealed class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<AuthenticatedUser?> ValidateCredentialsAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        if (user is null || !user.IsActive)
        {
            return null;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
            {
                _logger.LogWarning("Login denied for user {Username}: locked out", username);
            }

            return null;
        }

        return await BuildAuthenticatedUserAsync(user);
    }

    public async Task<AuthenticatedUser?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var userIdValue = _httpContextAccessor.HttpContext?.User.FindFirstValue(SecurityClaimTypes.Subject);
        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return null;
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user is null || !user.IsActive)
        {
            return null;
        }

        return await BuildAuthenticatedUserAsync(user);
    }

    private async Task<AuthenticatedUser> BuildAuthenticatedUserAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var permissions = ResolvePermissions(roles);

        return new AuthenticatedUser(
            user.Id,
            user.UserName ?? string.Empty,
            user.CompanyId,
            roles.ToArray(),
            permissions);
    }

    private static IReadOnlyCollection<string> ResolvePermissions(IEnumerable<string> roleNames)
    {
        var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var roleName in roleNames)
        {
            if (!Enum.TryParse<Role>(roleName, ignoreCase: true, out var role))
            {
                continue;
            }

            if (RolePermissions.Matrix.TryGetValue(role, out var rolePermissions))
            {
                permissions.UnionWith(rolePermissions);
            }
        }

        return permissions.ToArray();
    }
}
