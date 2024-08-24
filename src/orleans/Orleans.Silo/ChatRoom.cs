using System.Collections.Immutable;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Orleans.Runtime;
using Orleans.Utilities;

namespace Orleans.Silo;

[UsedImplicitly]
public class ChatRoom : Grain, IChatRoom
{
    private readonly IPersistentState<ChatRoomState> _state;
    private readonly ObserverManager<IChatRoomObserver> _observers;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ChatRoom(
        ILogger<ChatRoom> logger,
        [PersistentState("chatRoom", "blazorStore")] IPersistentState<ChatRoomState> state)
    {
        _state = state;
        _observers = new ObserverManager<IChatRoomObserver>(TimeSpan.FromHours(1), logger);
    }

    public async Task PostMessage(ChatMessage chatMessage)
    {
        _state.State.History.Add(chatMessage);
        await _state.WriteStateAsync();
        Console.WriteLine($"{nameof(ChatRoom)} is notifying all observers of the message: {chatMessage}");   
        await _observers.Notify(x => x.ReceiveMessage(chatMessage));
    }

    public Task<ReadOnlyCollection<ChatMessage>> Join(IChatRoomObserver observer)
    {
        _observers.Subscribe(observer, observer);
        return Task.FromResult(new ReadOnlyCollection<ChatMessage>(_state.State.History));
    }

    public Task Leave(IChatRoomObserver observer)
    {
        _observers.Unsubscribe(observer);
        return Task.CompletedTask;
    }

    public Task<ImmutableArray<ChatMessage>> GetHistory()
    {
        var history = ImmutableArray<ChatMessage>.Empty.AddRange(_state.State.History);
        return Task.FromResult(history);
    }
}