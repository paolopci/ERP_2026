using Microsoft.AspNetCore.Identity;

namespace Erp.Infrastructure.Persistence.Identity;

public sealed class AppUser : IdentityUser<Guid>
{
    public Guid CompanyId { get; set; }

    public bool IsActive { get; set; } = true;
}
