using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.Model;
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
    private List<UserData> Users { get; set; } = [];

    private IChatRoomObserver _chatRoomObserver = null!;

    private Timer _updateUsersOnline = null!;

    private const int UsersOnlineIntervalInSeconds = 5;
    private const int UserOnlineDefinitionInSeconds = -120;
    private const int RecentlyWroteMessageDefinitionInSeconds = -30;

    protected override Task OnInitializedAsync()
    {
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>(ChatRoomId);
        var observer = new ChatRoomObserver(_ => UpdateUsersOnline(), _ => UpdateUsersOnline());
        _chatRoomObserver = ClusterClient.CreateObjectReference<IChatRoomObserver>(observer);
        _ = chatRoomGrain.Join(_chatRoomObserver);

        _updateUsersOnline = new Timer(TimeSpan.FromSeconds(UsersOnlineIntervalInSeconds));
        _updateUsersOnline.Elapsed += async (_, _) => { await UpdateUsersOnline(); };
        _updateUsersOnline.Start();

        return UpdateUsersOnline();
    }

    private async Task UpdateUsersOnline()
    {
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>(ChatRoomId);
        var lastMessageByUser = await chatRoomGrain.GetLastMessageSentByUsers();
        var userData = lastMessageByUser
            .Where(x => x.Value > DateTimeOffset.UtcNow.AddSeconds(UserOnlineDefinitionInSeconds))
            .Select(x => new UserData
                { Username = x.Key, RecentlyWroteMessage = x.Value > DateTimeOffset.UtcNow.AddSeconds(RecentlyWroteMessageDefinitionInSeconds) })
            .ToList();
        Users.Clear();
        Users.AddRange(userData);
        await InvokeAsync(StateHasChanged);
    }

    private class UserData
    {
        public Username Username { get; set; } = null!;
        public bool RecentlyWroteMessage { get; set; }
    }
}