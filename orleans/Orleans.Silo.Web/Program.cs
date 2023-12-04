using Grains;
using Microsoft.AspNetCore.WebSockets;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleansClient(clientBuilder =>
{
    clientBuilder.UseLocalhostClustering();
});

builder.Services.AddWebSockets(_ => { });

var app = builder.Build();

app.MapGet("/connect/{wallboxId}", async (HttpContext context, string wallboxId) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var client = context.RequestServices.GetRequiredService<IClusterClient>();
        var grain = client.GetGrain<IWallboxGrain>(Guid.Parse(wallboxId));
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await grain.SayHello(webSocket);
        await context.Response.WriteAsync(
            "websocket connection established and message sent to grain. " +
            "should receive something back if all works well.");    
    }
    
    await context.Response.WriteAsync("not websocket request");
});

app.MapGet("/", () => "Hello World!");

app.UseWebSockets();

app.Run();