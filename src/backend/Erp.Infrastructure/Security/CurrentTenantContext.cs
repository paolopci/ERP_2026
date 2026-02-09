using System.Security.Claims;
using Erp.Application.Abstractions.Security;
using Erp.Contracts.Security;
using Microsoft.AspNetCore.Http;

namespace Erp.Infrastructure.Security;

public sealed class CurrentTenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? CompanyId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User.FindFirstValue(SecurityClaimTypes.CompanyId);
            return Guid.TryParse(value, out var companyId) ? companyId : null;
        }
    }
}
