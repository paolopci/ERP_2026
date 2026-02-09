namespace Erp.Contracts.Common;

public sealed record ApiError(
    string Code,
    string Message,
    string? Target = null);
