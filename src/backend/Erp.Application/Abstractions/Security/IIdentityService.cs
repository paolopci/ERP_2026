using Erp.Application.Common.Security;

namespace Erp.Application.Abstractions.Security;

public interface IIdentityService
{
    Task<AuthenticatedUser?> ValidateCredentialsAsync(string username, string password, CancellationToken cancellationToken);

    Task<AuthenticatedUser?> GetCurrentUserAsync(CancellationToken cancellationToken);
}
