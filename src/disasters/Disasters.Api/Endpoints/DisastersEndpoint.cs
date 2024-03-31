using Disasters.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Disasters.Api.Endpoints;

public static class DisastersEndpoint
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplication MapGetDisasters(this WebApplication builder)
    {
        builder.MapGet("/disasters", 
                async (
                HttpContext context, 
                IDisastersService disasterService,
                ILogger logger,
                [FromQuery] int page,
                [FromQuery] int pageSize) =>
                {
                    var disasters = await disasterService.GetDisasters(page, pageSize);
                    
                    await context.Response.WriteAsJsonAsync(disasters);
            })
            .RequireAuthorization("MaPol")
            .WithName("disasters")
            .WithDescription("Retrieves all disasters")
            .WithSummary("This endpoint retrieves all disasters. It requires the 'MaPol' authorization.")
            .WithOpenApi();

        return builder;
    }
    
    public record DisastersResponse(IEnumerable<DisasterVm> Disasters, int TotalCount, int Start, int End);
}