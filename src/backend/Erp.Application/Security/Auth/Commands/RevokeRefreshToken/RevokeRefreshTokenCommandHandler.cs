using Erp.Application.Abstractions.Security;
using MediatR;

namespace Erp.Application.Security.Auth.Commands.RevokeRefreshToken;

public sealed class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, bool>
{
    private readonly ITokenIssuer _tokenIssuer;

    public RevokeRefreshTokenCommandHandler(ITokenIssuer tokenIssuer)
    {
        _tokenIssuer = tokenIssuer;
    }

    public Task<bool> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return _tokenIssuer.RevokeRefreshTokenAsync(request.RefreshToken, request.UserId, cancellationToken);
    }
}
