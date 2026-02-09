using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Erp.Application.Abstractions.Security;
using Erp.Application.Common.Security;
using Erp.Contracts.Security;
using Erp.Infrastructure.Persistence;
using Erp.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Erp.Infrastructure.Security;

public sealed class JwtTokenIssuer : ITokenIssuer
{
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    private readonly UserManager<AppUser> _userManager;
    private readonly ErpDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtTokenOptions _options;

    public JwtTokenIssuer(
        UserManager<AppUser> userManager,
        ErpDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtTokenOptions> options)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _options = options.Value;
    }

    public async Task<TokenResponse> IssueTokensAsync(AuthenticatedUser user, CancellationToken cancellationToken)
    {
        var expiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(_options.ExpirationMinutes);
        var accessToken = GenerateAccessToken(user, expiresAtUtc);
        var refreshToken = await CreateAndPersistRefreshTokenAsync(user.UserId, cancellationToken);

        return new TokenResponse(accessToken, expiresAtUtc, refreshToken);
    }

    public async Task<TokenResponse?> RefreshTokensAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenHash = HashToken(refreshToken);
        var existingRefreshToken = await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == refreshTokenHash, cancellationToken);

        if (existingRefreshToken is null || !existingRefreshToken.IsActive)
        {
            return null;
        }

        var user = existingRefreshToken.User;
        if (!user.IsActive)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = ResolvePermissions(roles);
        var principal = new AuthenticatedUser(user.Id, user.UserName ?? string.Empty, user.CompanyId, roles.ToArray(), permissions);
        var expiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(_options.ExpirationMinutes);
        var accessToken = GenerateAccessToken(principal, expiresAtUtc);

        existingRefreshToken.RevokedAtUtc = DateTimeOffset.UtcNow;
        existingRefreshToken.RevokedByIp = GetRequestIp();

        var newRefreshTokenRaw = GenerateRefreshToken();
        var newRefreshTokenHash = HashToken(newRefreshTokenRaw);
        existingRefreshToken.ReplacedByTokenHash = newRefreshTokenHash;

        var rotatedRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = newRefreshTokenHash,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(_options.RefreshTokenDays),
            CreatedByIp = GetRequestIp()
        };

        _dbContext.RefreshTokens.Add(rotatedRefreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TokenResponse(accessToken, expiresAtUtc, newRefreshTokenRaw);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, Guid? userId, CancellationToken cancellationToken)
    {
        var refreshTokenHash = HashToken(refreshToken);
        var existingRefreshToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == refreshTokenHash, cancellationToken);

        if (existingRefreshToken is null || !existingRefreshToken.IsActive)
        {
            return false;
        }

        if (userId.HasValue && existingRefreshToken.UserId != userId.Value)
        {
            return false;
        }

        existingRefreshToken.RevokedAtUtc = DateTimeOffset.UtcNow;
        existingRefreshToken.RevokedByIp = GetRequestIp();
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private string GenerateAccessToken(AuthenticatedUser user, DateTimeOffset expiresAtUtc)
    {
        var claims = new List<Claim>
        {
            new(SecurityClaimTypes.Subject, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(SecurityClaimTypes.CompanyId, user.CompanyId.ToString())
        };

        claims.AddRange(user.Roles.Select(role => new Claim(SecurityClaimTypes.Role, role)));
        claims.AddRange(user.Permissions.Select(permission => new Claim(SecurityClaimTypes.Permission, permission)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAtUtc.UtcDateTime,
            signingCredentials: credentials);

        return TokenHandler.WriteToken(jwtToken);
    }

    private async Task<string> CreateAndPersistRefreshTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        var rawRefreshToken = GenerateRefreshToken();
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = HashToken(rawRefreshToken),
            CreatedAtUtc = DateTimeOffset.UtcNow,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(_options.RefreshTokenDays),
            CreatedByIp = GetRequestIp()
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return rawRefreshToken;
    }

    private static string GenerateRefreshToken()
    {
        Span<byte> bytes = stackalloc byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string HashToken(string value)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(hash);
    }

    private string? GetRequestIp()
    {
        return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
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
