namespace Erp.Contracts.Common;

public sealed record SortField(
    string Field,
    SortDirection Direction = SortDirection.Asc);

public enum SortDirection
{
    Asc = 0,
    Desc = 1
}
