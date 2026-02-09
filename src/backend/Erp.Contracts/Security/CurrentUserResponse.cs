namespace Erp.Contracts.Security;

public sealed record CurrentUserResponse(
    Guid UserId,
    string Username,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions,
    Guid CompanyId);
