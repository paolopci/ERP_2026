using System.Security.Claims;
using Erp.Application.Security.Auth.Commands.Login;
using Erp.Application.Security.Auth.Commands.RefreshToken;
using Erp.Application.Security.Auth.Commands.RevokeRefreshToken;
using Erp.Application.Security.Auth.Queries.GetMe;
using Erp.Contracts.Common;
using Erp.Contracts.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Erp.Api.Controllers;

public sealed class AuthController : ApiV1Controller
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [EnableRateLimiting("auth-endpoints")]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiEnvelope<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new LoginCommand(request.Username, request.Password), cancellationToken);
        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(ApiEnvelope<TokenResponse>.Success(result, traceId: CurrentTraceId));
    }

    [AllowAnonymous]
    [EnableRateLimiting("auth-endpoints")]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiEnvelope<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new RefreshTokenCommand(request.RefreshToken), cancellationToken);
        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(ApiEnvelope<TokenResponse>.Success(result, traceId: CurrentTraceId));
    }

    [Authorize]
    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Revoke([FromBody] RevokeTokenRequest request, CancellationToken cancellationToken)
    {
        var userId = Guid.TryParse(User.FindFirstValue(SecurityClaimTypes.Subject), out var parsedUserId)
            ? parsedUserId
            : (Guid?)null;

        var revoked = await _sender.Send(new RevokeRefreshTokenCommand(request.RefreshToken, userId), cancellationToken);
        return revoked ? NoContent() : Unauthorized();
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiEnvelope<CurrentUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMeQuery(), cancellationToken);
        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(ApiEnvelope<CurrentUserResponse>.Success(result, traceId: CurrentTraceId));
    }
}
