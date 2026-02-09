using Erp.Application.Abstractions.Security;
using Erp.Contracts.Security;
using MediatR;

namespace Erp.Application.Security.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse?>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenIssuer _tokenIssuer;

    public LoginCommandHandler(IIdentityService identityService, ITokenIssuer tokenIssuer)
    {
        _identityService = identityService;
        _tokenIssuer = tokenIssuer;
    }

    public async Task<TokenResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.ValidateCredentialsAsync(request.Username, request.Password, cancellationToken);
        if (user is null)
        {
            return null;
        }

        return await _tokenIssuer.IssueTokensAsync(user, cancellationToken);
    }
}
