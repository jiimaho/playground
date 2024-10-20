using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Orleans.Silo;

namespace OrleansBlazor.Components.Pages;

[UsedImplicitly]
public partial class Chat : ComponentBase
{
    [Inject]
    private IClusterClient ClusterClient { get; init; } = null!;
    
    private const string ChatRoomId = "all";
    
    private IChatRoomObserver? _chatRoomObserver;
    
    private List<ChatMessage> Messages { get; set; } = [];
    public List<string> UsersOnline { get; set; } = [];
    private string? Message { get; set; }
    private string? Username { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (OperatingSystem.IsBrowser())
        {
            Console.WriteLine("Running in a Blazor WebAssembly environment.");
        }
        else
        {
            Console.WriteLine("Running in a Blazor Server environment.");
        }

        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>(ChatRoomId);
        var observer = new ChatRoomObserver(async msg =>
        {
            Messages.Add(msg);
            var usersOnline = await chatRoomGrain.GetUsersOnline();
            UsersOnline.Clear();
            UsersOnline.AddRange(usersOnline);
            await InvokeAsync(StateHasChanged);
            Console.WriteLine("Got msg!");
        }, async users => {
            UsersOnline.Clear();
            UsersOnline.AddRange(users);
            await InvokeAsync(StateHasChanged);
            Console.WriteLine("Got users!");
        });
        _chatRoomObserver = ClusterClient.CreateObjectReference<IChatRoomObserver>(observer);
        await chatRoomGrain.Join(_chatRoomObserver);
        var history = await chatRoomGrain.GetHistory();
        Messages.AddRange(history.ToList());
        Console.WriteLine("Got grain and running");
    }
    
    protected async Task Clear()
    {
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>(ChatRoomId);
        await chatRoomGrain.Clear();
        Messages.Clear();
    }

    protected async Task SendMessage()
    {
        Console.WriteLine(
            $"Am i executed on the server or in the browser? Environment.OSVersion: {Environment.OSVersion}, Process: {Process.GetCurrentProcess().Id}");
        if (Username is null || Message is null)
            return;
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>(ChatRoomId);
        var chatMessage = new ChatMessage(User: Username, Message: Message);
        await chatRoomGrain.PostMessage(chatMessage);
    }
}