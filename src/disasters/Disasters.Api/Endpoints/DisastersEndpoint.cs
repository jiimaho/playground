using System.Diagnostics;
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
                    
                    using (var activity = Tracing.DisastersApi.StartActivity("Crunching...", ActivityKind.Internal))
                    {
                        await Task.Delay(2000);
                    }
                    
                    var at = Tracing.DisastersApi.StartActivity("GetDisasters");
                    var disasters = await disasterService.GetDisasters();
                    if (at == null) logger.Warning("at is null!!!");
                    at?.Dispose();

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