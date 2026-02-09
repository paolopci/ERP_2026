using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiV1Controller : ControllerBase
{
    protected string CurrentTraceId => HttpContext.TraceIdentifier;
}
