using Erp.Contracts.Security;
using MediatR;

namespace Erp.Application.Security.Auth.Commands.Login;

public sealed record LoginCommand(string Username, string Password) : IRequest<TokenResponse?>;
