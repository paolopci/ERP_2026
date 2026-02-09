using MediatR;

namespace Erp.Application.Security.Auth.Commands.RevokeRefreshToken;

public sealed record RevokeRefreshTokenCommand(string RefreshToken, Guid? UserId) : IRequest<bool>;
