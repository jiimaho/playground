using Disasters.Api.Services;

namespace Disasters.Api.Endpoints;

public static class MarkSafeEndpoint
{
    public static WebApplication MapPostMarkSafe(this WebApplication builder)
    {
        builder.MapPost("/disasters/mark-safe", RouteHandler)
            .RequireAuthorization("MaPol")
            .WithName("mark-safe")
            .WithDescription("Marks a disaster as safe")
            .WithSummary("This endpoint marks a disaster as safe. It requires the 'MaPol' authorization.")
            .WithOpenApi();

        return builder;

        Task RouteHandler(HttpContext context, IDisastersService disasterService, ILogger logger, MarkSafeVm vm)
        {
            return disasterService.MarkSafe(vm);
        }
    }
}