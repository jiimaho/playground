using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Orleans.Silo;
using Orleans.Silo.Primitives;

namespace OrleansBlazor.Components.Pages;

public partial class UsersOnline : ComponentBase
{
    [Parameter, Required]
    public string ChatRoomId { get; set; } = null!;

    [Inject]
    private IClusterClient ClusterClient { get; init; } = null!;

    private List<Username> AllUsersOnline { get; set; } = [];

    private IChatRoomObserver _chatRoomObserver = null!;

    protected override Task OnInitializedAsync()
    {
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>(ChatRoomId);
        var observer = new ChatRoomObserver(async _ =>
        {
            var lastMessageByUser = await chatRoomGrain.GetLastMessageSentByUsers();
            var online = lastMessageByUser.Where(m => m.Value > DateTimeOffset.Now.AddMinutes(-1)).Select(m => m.Key);
            AllUsersOnline.Clear();
            AllUsersOnline.AddRange(online);
            await InvokeAsync(StateHasChanged);
        }, async _ =>
        {
            var lastMessageByUser = await chatRoomGrain.GetLastMessageSentByUsers();
            var online = lastMessageByUser.Where(m => m.Value > DateTimeOffset.Now.AddMinutes(1)).Select(m => m.Key);
            AllUsersOnline.Clear();
            AllUsersOnline.AddRange(online);
            await InvokeAsync(StateHasChanged);
        });

        _chatRoomObserver = ClusterClient.CreateObjectReference<IChatRoomObserver>(observer);

        _ = chatRoomGrain.Join(_chatRoomObserver);

        return Task.CompletedTask;
    }
}