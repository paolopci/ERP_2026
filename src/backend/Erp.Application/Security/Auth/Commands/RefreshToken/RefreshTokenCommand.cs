using Erp.Contracts.Security;
using MediatR;

namespace Erp.Application.Security.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponse?>;
