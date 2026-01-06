using System.Net;
using System.Text.Json;
using FluentValidation;
using TaskManagement.Service.Exceptions; // ✅ הוסף את זה

namespace TaskManagement.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An error occurred while processing your request.";
        Dictionary<string, string[]>? errors = null;

        switch (exception)
        {
            // ✅ FluentValidation errors -> 400 with field errors
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Validation failed";
                errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                break;

            // ✅ Service layer exceptions -> proper HTTP mapping
            case NotFoundException nf:
                statusCode = HttpStatusCode.NotFound;
                message = nf.Message;
                break;

            case ConflictException cf:
                statusCode = HttpStatusCode.Conflict;
                message = cf.Message;
                break;

            // Existing mappings (keep if you still throw these)
            case KeyNotFoundException:
            case ArgumentNullException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Unauthorized access";
                break;
        }

        return WriteErrorResponse(context, statusCode, message, errors);
    }

    private static Task WriteErrorResponse(
        HttpContext context,
        HttpStatusCode statusCode,
        string message,
        Dictionary<string, string[]>? errors = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            message,
            errors
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(json);
    }
}
