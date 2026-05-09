using System.Net;
using System.Text.Json;

namespace StudyQuest.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled exception for {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);

            await HandleExceptionAsync(context, ex, _environment.IsDevelopment());
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, bool includeDetails)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, detail) = exception switch
        {
            UnauthorizedAccessException ex => (HttpStatusCode.Unauthorized, "Unauthorized access.", includeDetails ? ex.Message : null),
            ArgumentException ex => (HttpStatusCode.BadRequest, "Invalid request.", includeDetails ? ex.Message : null),
            InvalidOperationException ex => (HttpStatusCode.BadRequest, "Invalid operation.", includeDetails ? ex.Message : null),
            KeyNotFoundException ex => (HttpStatusCode.NotFound, "Resource not found.", includeDetails ? ex.Message : null),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.", includeDetails ? exception.Message : null)
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            detail,
            traceId = context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
