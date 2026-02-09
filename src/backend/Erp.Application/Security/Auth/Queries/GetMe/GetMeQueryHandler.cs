using Erp.Application.Abstractions.Security;
using Erp.Contracts.Security;
using MediatR;

namespace Erp.Application.Security.Auth.Queries.GetMe;

public sealed class GetMeQueryHandler : IRequestHandler<GetMeQuery, CurrentUserResponse?>
{
    private readonly IIdentityService _identityService;

    public GetMeQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<CurrentUserResponse?> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetCurrentUserAsync(cancellationToken);
        if (user is null)
        {
            return null;
        }

        return new CurrentUserResponse(user.UserId, user.Username, user.Roles, user.Permissions, user.CompanyId);
    }
}
