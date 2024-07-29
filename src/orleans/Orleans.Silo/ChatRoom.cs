using System.Collections.ObjectModel;
using Orleans.Utilities;

namespace Orleans.Silo;

[Alias("IChatRoom")]
public interface IChatRoom : IGrainWithStringKey
{
    [Alias("PostMessage")]
    Task PostMessage(ChatMessage chatMessage);
    [Alias("Join")]
    Task<ReadOnlyCollection<ChatMessage>> Join(IChatRoomObserver observer);
    [Alias("Leave")]
    Task Leave(IChatRoomObserver observer);
}

public class ChatRoom(ILogger<ChatRoom> logger) : Grain,IChatRoom
{
    private readonly Collection<ChatMessage> _history = [];
    private readonly ObserverManager<IChatRoomObserver> _observers = new(TimeSpan.FromMinutes(5), logger);
    
    public async Task PostMessage(ChatMessage chatMessage)
    {
        _history.Add(chatMessage);
        Console.WriteLine($"{nameof(ChatRoom)} is notifying all observers of the message: {chatMessage}");   
        await _observers.Notify(x => x.ReceiveMessage(chatMessage));
    }

    public Task<ReadOnlyCollection<ChatMessage>> Join(IChatRoomObserver observer)
    {
        _observers.Subscribe(observer, observer);
        return Task.FromResult(_history.AsReadOnly());
    }

    public Task Leave(IChatRoomObserver observer)
    {
        _observers.Unsubscribe(observer);
        return Task.CompletedTask;
    }
}

[Alias("ChatMessage")]
[GenerateSerializer]
public record ChatMessage(string User, string Message);