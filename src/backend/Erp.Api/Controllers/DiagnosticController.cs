using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Controllers;

[ApiController]
[AllowAnonymous]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/diagnostic")]
public sealed class DiagnosticController : ControllerBase
{
    [HttpGet("fail")]
    public IActionResult Fail()
    {
        throw new InvalidOperationException("Diagnostic failure endpoint.");
    }
}
