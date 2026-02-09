using Erp.Application.MasterData.Customers.Commands.CreateCustomer;
using Erp.Application.MasterData.Customers.Queries.GetCustomerById;
using Erp.Contracts.Common;
using Erp.Contracts.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Controllers;

[Authorize]
public sealed class MasterDataController : ApiV1Controller
{
    private readonly ISender _sender;

    public MasterDataController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("customers")]
    [Authorize(Policy = PermissionCatalog.AnagraficheWrite)]
    [ProducesResponseType(typeof(ApiEnvelope<Guid>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiEnvelope<Guid>>> CreateCustomer(
        [FromBody] CreateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);
        return Ok(ApiEnvelope<Guid>.Success(id, traceId: CurrentTraceId));
    }

    [HttpGet("customers/{id:guid}")]
    [Authorize(Policy = PermissionCatalog.AnagraficheRead)]
    [ProducesResponseType(typeof(ApiEnvelope<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _sender.Send(new GetCustomerByIdQuery(id), cancellationToken);
        if (customer is null)
        {
            return NotFound();
        }

        return Ok(ApiEnvelope<object>.Success(customer, traceId: CurrentTraceId));
    }
}
