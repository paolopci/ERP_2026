using Erp.Application.Common.Security;
using Erp.Contracts.Security;

namespace Erp.Application.Abstractions.Security;

public interface ITokenIssuer
{
    Task<TokenResponse> IssueTokensAsync(AuthenticatedUser user, CancellationToken cancellationToken);

    Task<TokenResponse?> RefreshTokensAsync(string refreshToken, CancellationToken cancellationToken);

    Task<bool> RevokeRefreshTokenAsync(string refreshToken, Guid? userId, CancellationToken cancellationToken);
}
