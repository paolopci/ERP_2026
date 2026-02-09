using Erp.Application.Abstractions.Security;
using Erp.Contracts.Security;
using MediatR;

namespace Erp.Application.Security.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse?>
{
    private readonly ITokenIssuer _tokenIssuer;

    public RefreshTokenCommandHandler(ITokenIssuer tokenIssuer)
    {
        _tokenIssuer = tokenIssuer;
    }

    public Task<TokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return _tokenIssuer.RefreshTokensAsync(request.RefreshToken, cancellationToken);
    }
}
