using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Orleans.Silo;
using Orleans.Silo.Primitives;

namespace OrleansBlazor.Components.Pages;

[UsedImplicitly]
public partial class Chat : ComponentBase
{
    [Inject]
    private IClusterClient ClusterClient { get; init; } = null!;
    
    private const string ChatRoomId = "all";
    
    private IChatRoomObserver? _chatRoomObserver;
    
    private List<Orleans.Silo.Primitives.ChatMessage> Messages { get; set; } = [];
    // public List<string> UsersOnline { get; set; } = [];
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
        var observer = new ChatRoomObserver(msg =>
        {
            Messages.Add(msg);
            Console.WriteLine("Got msg!");
            return InvokeAsync(StateHasChanged);
        }, _ => Task.CompletedTask);
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
        var chatMessage = new Orleans.Silo.Primitives.ChatMessage(new Username(Username), Message);
        await chatRoomGrain.PostMessage(chatMessage);
    }
}