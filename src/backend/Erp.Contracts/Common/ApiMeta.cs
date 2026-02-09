namespace Erp.Contracts.Common;

public sealed record ApiMeta(
    int? Page = null,
    int? PageSize = null,
    int? TotalCount = null,
    IDictionary<string, string>? Additional = null)
{
    public static ApiMeta Empty { get; } = new();
}
