namespace Erp.Contracts.Common;

public sealed record PagedRequest(
    int Page = 1,
    int PageSize = 20,
    IReadOnlyList<SortField>? Sort = null,
    IReadOnlyList<FilterField>? Filters = null);
