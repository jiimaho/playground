using System.Reactive.Linq;
using Disasters.Api.Services;

namespace Disasters.Api.Endpoints;

public static class DisastersEndpoint
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplication MapDisasters(this WebApplication builder)
    {
        builder.MapGet("/disasters", 
                async (
                HttpContext context, 
                IDisastersService disasterService,
                ILogger logger) =>
                {
                    var disasters = await disasterService.GetDisasters();

                    var obs = Observable.FromAsync(disasterService.GetDisasters)
                        .Retry(0)
                        .Subscribe(x => Console.WriteLine("Observable done"));

                    obs.Dispose();

                    await context.Response.WriteAsJsonAsync(disasters);
            })
            .RequireAuthorization("MaPol")
            .WithName("disasters")
            .WithDescription("Retrieves all disasters")
            .WithSummary("This endpoint retrieves all disasters. It requires the 'MaPol' authorization.")
            .WithOpenApi();

        return builder;
    }
}