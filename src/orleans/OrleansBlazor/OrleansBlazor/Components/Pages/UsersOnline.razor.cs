using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Orleans.Silo;
using Orleans.Silo.Primitives;
using Timer = System.Timers.Timer;

namespace OrleansBlazor.Components.Pages;

public partial class UsersOnline : ComponentBase
{
    // Parameters
    [Parameter, Required]
    public string ChatRoomId { get; set; } = null!;

    // DI
    [Inject]
    private IClusterClient ClusterClient { get; init; } = null!;

    // State
    private List<Username> AllUsersOnline { get; set; } = [];

    private IChatRoomObserver _chatRoomObserver = null!;

    private Timer _updateUsersOnline = null!;

    private const int UsersOnlineIntervalInSeconds = 5;
    private const int UserOnlineDefinitionInMinutes = -1;

    protected override Task OnInitializedAsync()
    {
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>(ChatRoomId);
        var observer = new ChatRoomObserver(_ => UpdateUsersOnline(), _ => UpdateUsersOnline());
        _chatRoomObserver = ClusterClient.CreateObjectReference<IChatRoomObserver>(observer);
        _ = chatRoomGrain.Join(_chatRoomObserver);

        _updateUsersOnline = new Timer(TimeSpan.FromSeconds(UsersOnlineIntervalInSeconds));
        _updateUsersOnline.Elapsed += async (_, _) => { await UpdateUsersOnline(); };
        _updateUsersOnline.Start();

        return Task.CompletedTask;
    }

    private async Task UpdateUsersOnline()
    {
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>(ChatRoomId);
        var lastMessageByUser = await chatRoomGrain.GetLastMessageSentByUsers();
        var online = lastMessageByUser.Where(m => m.Value > DateTimeOffset.Now.AddMinutes(UserOnlineDefinitionInMinutes)).Select(m => m.Key);
        AllUsersOnline.Clear();
        AllUsersOnline.AddRange(online);
        await InvokeAsync(StateHasChanged);
    }
}