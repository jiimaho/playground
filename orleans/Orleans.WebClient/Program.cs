using System.Buffers;
using System.Net.WebSockets;
using System.Text;
using Grains;
using Microsoft.AspNetCore.WebSockets;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Silo.Web;
using Orleans.Streams;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleansClient(clientBuilder =>
{
    clientBuilder.UseLocalhostClustering()
        .AddMemoryStreams("in-memory");
});

builder.Services.AddWebSockets(_ => { });

builder.Services.AddHostedService<StatusListenerBackgroundService>();

var app = builder.Build();

app.MapGet("/connect/{wallboxId}", async (HttpContext context, string wallboxId) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var client = context.RequestServices.GetRequiredService<IClusterClient>();
        var handle = await SubscribeToWallboxAndForwardToSocket(client, wallboxId, webSocket);

        var task = Task.Run(() => LoopReceiveOnSocketAndPrintConsole(webSocket));

        await task;
        await handle.UnsubscribeAsync();
        return;
    }
    
    await context.Response.WriteAsync("not websocket request");
});

app.MapGet("/", () => "Hello World!");

app.UseWebSockets();

app.Run();

async Task LoopReceiveOnSocketAndPrintConsole(WebSocket webSocket1)
{
    while (webSocket1.State == WebSocketState.Open)
    {
        var receivePool = ArrayPool<byte>.Shared;
        var receiveBuffer = receivePool.Rent(1024 * 8);
        try
        {
            var receiveResult = await webSocket1.ReceiveAsync(receiveBuffer, CancellationToken.None);
            var receiveString = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
            Console.WriteLine(receiveString);
        }
        catch (OperationCanceledException e)
        {
        }
    }
}

async Task<StreamSubscriptionHandle<WallboxStatusEvent>> SubscribeToWallboxAndForwardToSocket(IClusterClient clusterClient, string s, WebSocket webSocket1)
{
    return await clusterClient.GetStreamProvider("in-memory")
        .GetStream<WallboxStatusEvent>(StreamId.Create("wallbox-status", Guid.Parse(s)))
        .SubscribeAsync(async (message, token) =>
        {
            var sendPool = ArrayPool<byte>.Shared;
            var sendBuffer = sendPool.Rent(1024 * 8);
            var sendString = Encoding.UTF8.GetBytes(message.ToString());
            Array.Copy(sendString, sendBuffer, sendString.Length);
            await webSocket1.SendAsync(sendBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
        });
}