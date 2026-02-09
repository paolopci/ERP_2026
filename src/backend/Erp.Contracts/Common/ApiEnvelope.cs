namespace Erp.Contracts.Common;

public sealed record ApiEnvelope<T>(
    T? Data,
    IReadOnlyList<ApiError> Errors,
    ApiMeta Meta,
    string TraceId)
{
    public static ApiEnvelope<T> Success(T data, ApiMeta? meta = null, string traceId = "") =>
        new(data, [], meta ?? ApiMeta.Empty, traceId);

    public static ApiEnvelope<T> Failure(IReadOnlyList<ApiError> errors, ApiMeta? meta = null, string traceId = "") =>
        new(default, errors, meta ?? ApiMeta.Empty, traceId);
}
