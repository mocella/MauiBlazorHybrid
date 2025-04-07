using Microsoft.AspNetCore.Diagnostics;

namespace MauiBlazorHybrid.Api.MIddleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected error occurred");

        return false; //allow pipeline to continue along and handle the exception/response as appropriate
    }
}
