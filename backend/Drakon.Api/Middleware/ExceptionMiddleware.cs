using System.Text.Json;
using Drakon.Application.Common.Exceptions;

namespace Drakon.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            var (status, message) = ex switch
            {
                InvalidCredentialsException => (401, ex.Message),
                KeyNotFoundException => (404, ex.Message),
                UnauthorizedAccessException => (403, ex.Message),
                InvalidOperationException => (400, ex.Message),
                _ => (500, "Error interno del servidor.")
            };

            if (status == 500) _logger.LogError(ex, "Unhandled exception");

            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(new { status, message }));
        }
    }
}