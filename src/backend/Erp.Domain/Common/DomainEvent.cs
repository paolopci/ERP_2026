namespace Erp.Domain.Common;

public abstract record DomainEvent(Guid EventId, DateTimeOffset OccurredAtUtc);
