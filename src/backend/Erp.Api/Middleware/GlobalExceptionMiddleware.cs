using System.Text.Json;
using Erp.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Erp.Api.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            _logger.LogWarning(exception, "Validation error while executing request");
            await WriteProblemDetailsAsync(context, StatusCodes.Status400BadRequest, "Validation error", exception.Message, exception.Errors);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception while executing request");
            await WriteProblemDetailsAsync(context, StatusCodes.Status500InternalServerError, "Unexpected error", "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemDetailsAsync(
        HttpContext context,
        int statusCode,
        string title,
        string detail,
        IReadOnlyDictionary<string, string[]>? errors = null)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;
        if (errors is not null)
        {
            problem.Extensions["errors"] = errors;
        }

        var payload = JsonSerializer.Serialize(problem);
        await context.Response.WriteAsync(payload);
    }
}
