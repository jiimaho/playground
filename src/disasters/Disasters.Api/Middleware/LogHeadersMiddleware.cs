namespace Disasters.Api.Middleware;

public class LogHeadersMiddleware(RequestDelegate next, ILogger logger)
{
    private readonly ILogger _logger = logger.ForContext<LogHeadersMiddleware>();
    
    public Task InvokeAsync(HttpContext context)
    {
        _logger.Information("Begin logging headers...");
        foreach (var keyValuePair in context.Request.Headers.ToList())
        {
            _logger.Information(
                "Header {Key}: {Value}", 
                !string.IsNullOrWhiteSpace(keyValuePair.Key) ? keyValuePair.Key : "<unknown>", 
                keyValuePair.Value);
        }
        _logger.Information("End logging headers...");

        return next(context);
    }
}