using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Orleans.Silo;

namespace OrleansBlazor.Components.Pages;

[UsedImplicitly]
public partial class Chat : ComponentBase
{
    private IChatRoomObserver? _o;
    protected List<ChatMessage> Messages { get; set; } = [];

    public string? Message { get; set; }
    public string? Username { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await Task.Delay(2000);
        if (OperatingSystem.IsBrowser())
        {
            Console.WriteLine("Running in a Blazor WebAssembly environment.");
        }
        else
        {
            Console.WriteLine("Running in a Blazor Server environment.");
        }

        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>("all");
        var observer = new ChatRoomObserver(async msg =>
        {
            Messages.Add(msg);
            await InvokeAsync(StateHasChanged);
            Console.WriteLine("Got msg!");
        });
        _o = ClusterClient.CreateObjectReference<IChatRoomObserver>(observer);
        await chatRoomGrain.Join(_o);
        var history = await chatRoomGrain.GetHistory();
        Messages.AddRange(history.ToList());
        Console.WriteLine("Got grain and running");
    }

    protected async Task SendMessage()
    {
        Console.WriteLine(
            $"Am i executed on the server or in the browser? Environment.OSVersion: {Environment.OSVersion}, Process: {Process.GetCurrentProcess().Id}");
        if (Username is null || Message is null)
            return;
        var chatRoomGrain = ClusterClient.GetGrain<IChatRoom>("all");
        var chatMessage = new ChatMessage(User: Username, Message: Message);
        await chatRoomGrain.PostMessage(chatMessage);
    }
}