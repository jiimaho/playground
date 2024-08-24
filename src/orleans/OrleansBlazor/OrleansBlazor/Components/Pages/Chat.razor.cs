using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Orleans.Silo;

namespace OrleansBlazor.Components.Pages;

[UsedImplicitly]
public partial class Chat : ComponentBase
{
    private IChatRoomObserver? _o;
    protected List<string> Messages { get; } = new();

    public string? Message { get; set; }
    public string? Username { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>("all");
        var observer = new ChatRoomObserver(async msg =>
        {
            Messages.Add(msg.ToString());
            await InvokeAsync(StateHasChanged);
            Console.WriteLine("Got msg!");
        });
        _o = ClusterClient.CreateObjectReference<IChatRoomObserver>(observer);
        await chatRoomGrain.Join(_o);
        var history = await chatRoomGrain.GetHistory();
        Messages.AddRange(history.Select(m => m.ToString()));
        Console.WriteLine("Got grain and running");
    }

    protected async Task SendMessage()
    {
        Console.WriteLine($"Am i executed on the server or in the browser? Environment.OSVersion: {Environment.OSVersion}, Process: {Process.GetCurrentProcess().Id}");
        if (Username is null ||  Message is null)
            return;
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>("all");
        var chatMessage = new ChatMessage(User: Username, Message: Message);
        await chatRoomGrain.PostMessage(chatMessage);
    }
}