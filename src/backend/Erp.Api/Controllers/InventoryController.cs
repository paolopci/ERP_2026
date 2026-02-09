using Erp.Contracts.Common;
using Erp.Contracts.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Controllers;

[Authorize]
public sealed class InventoryController : ApiV1Controller
{
    [HttpGet("summary")]
    [Authorize(Policy = PermissionCatalog.MagazzinoRead)]
    [ProducesResponseType(typeof(ApiEnvelope<object>), StatusCodes.Status200OK)]
    public ActionResult<ApiEnvelope<object>> GetSummary()
    {
        var payload = new
        {
            Message = "Inventory module baseline endpoint",
            TimestampUtc = DateTimeOffset.UtcNow
        };

        return Ok(ApiEnvelope<object>.Success(payload, traceId: CurrentTraceId));
    }
}
