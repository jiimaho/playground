using Chatty.Silo;
using Chatty.Web.Components;
using Chatty.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Chatty.Web.BackgroundService;

public class UserOnlineNotifier(
    IClusterClient clusterClient,
    IHubContext<UserOnlineHub, IUserOnlineHub> hubContext): Microsoft.Extensions.Hosting.BackgroundService, IAsyncDisposable
{
    private IChatRoomObserver _chatRoomObserver;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(5000, stoppingToken);
        var chatRoom = clusterClient.GetGrain<IChatRoom>("all");
        var observer = new ChatRoomObserver(_ => Task.CompletedTask, _ => Task.CompletedTask, async username =>
        {
            await hubContext.Clients.All.UserOnline(username.ToString());
        });
        _chatRoomObserver = clusterClient.CreateObjectReference<IChatRoomObserver>(observer);
        await chatRoom.Join(_chatRoomObserver);
    }

    public async ValueTask DisposeAsync()
    {
        var chatRoom = clusterClient.GetGrain<IChatRoom>("all");
        await chatRoom.Leave(_chatRoomObserver);
    }
}