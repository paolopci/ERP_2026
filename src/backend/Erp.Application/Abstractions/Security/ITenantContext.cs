namespace Erp.Application.Abstractions.Security;

public interface ITenantContext
{
    Guid? CompanyId { get; }
}
