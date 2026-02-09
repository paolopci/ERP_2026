namespace Erp.Contracts.Audit;

public sealed record AuditRecord(
    string Entity,
    Guid EntityId,
    AuditAction Action,
    Guid UserId,
    DateTimeOffset TimestampUtc,
    IReadOnlyDictionary<string, string?> OldValues,
    IReadOnlyDictionary<string, string?> NewValues);

public enum AuditAction
{
    Create = 0,
    Update = 1,
    Delete = 2
}
