using Microsoft.Extensions.Hosting;
using Orleans.Silo;

namespace Orleans.ConsoleConsumer;

public class ChatBackgroundService(IClusterClient clusterClient) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine($"Starting {nameof(ChatBackgroundService)}");
        var chatRoom = clusterClient.GetGrain<IChatRoom>("all");
        var chatRoomConsoleWriter = new ChatRoomConsoleWriter();
        var observerReference = clusterClient.CreateObjectReference<IChatRoomObserver>(chatRoomConsoleWriter);
        await chatRoom.Join(observerReference);
        await Task.Delay(TimeSpan.FromMinutes(10), CancellationToken.None);
        await chatRoom.Leave(observerReference);
    }
}