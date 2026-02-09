namespace Erp.Application.Common.Security;

public sealed record AuthenticatedUser(
    Guid UserId,
    string Username,
    Guid CompanyId,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions);
