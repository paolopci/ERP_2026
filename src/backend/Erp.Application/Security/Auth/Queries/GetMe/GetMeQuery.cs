using Erp.Contracts.Security;
using MediatR;

namespace Erp.Application.Security.Auth.Queries.GetMe;

public sealed record GetMeQuery : IRequest<CurrentUserResponse?>;
