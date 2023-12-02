using System.Reactive.Linq;
using Disasters.Api.Disasters;

namespace Disasters.Api.Endpoints;

public static class DisasterEndpoints
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplication MapEndpoints(this WebApplication builder)
    {
        builder.MapGet("/disasters", async context =>
        {
            var disasterService = context.RequestServices.GetRequiredService<IDisastersService>();
            var disasters = await disasterService.GetDisasters();
    
            var obs =  Observable.FromAsync(disasterService.GetDisasters)
                .Retry(0)
                .Subscribe(x => Console.WriteLine("Observable done"));
    
            obs.Dispose();

            await context.Response.WriteAsJsonAsync(disasters);
        }).RequireAuthorization("MaPol");

        return builder;
    }
}