namespace Erp.Contracts.Common;

public sealed record FilterField(
    string Field,
    FilterOperator Operator,
    string Value);

public enum FilterOperator
{
    Eq = 0,
    Ne = 1,
    Gt = 2,
    Ge = 3,
    Lt = 4,
    Le = 5,
    Contains = 6,
    StartsWith = 7,
    EndsWith = 8,
    In = 9
}
