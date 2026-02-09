using Erp.Contracts.Common;
using Erp.Contracts.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Controllers;

[Authorize]
public sealed class SalesController : ApiV1Controller
{
    [HttpGet("documents")]
    [Authorize(Policy = PermissionCatalog.VenditeRead)]
    [ProducesResponseType(typeof(ApiEnvelope<object>), StatusCodes.Status200OK)]
    public ActionResult<ApiEnvelope<object>> GetDocuments()
    {
        var payload = new
        {
            Message = "Sales module baseline endpoint",
            TimestampUtc = DateTimeOffset.UtcNow
        };

        return Ok(ApiEnvelope<object>.Success(payload, traceId: CurrentTraceId));
    }
}
