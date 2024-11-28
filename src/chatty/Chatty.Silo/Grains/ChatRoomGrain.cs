using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Chatty.Silo.Configuration;
using Chatty.Silo.Primitives;
using JetBrains.Annotations;
using Orleans.Runtime;
using Orleans.Utilities;

namespace Chatty.Silo.Grains;

[UsedImplicitly]
public class ChatRoomGrain : Grain, IChatRoom 
{
    private readonly IPersistentState<ChatRoomGrainState> _state;
    private readonly ObserverManager<IChatRoomObserver> _observers;

    private readonly ChatRoomVolatileState _volatileState = new([]);
    
    private const string PersistentStateName = "chatRoom";

    // ReSharper disable once ConvertToPrimaryConstructor
    public ChatRoomGrain(
        ILogger<ChatRoomGrain> logger,
        [PersistentState(PersistentStateName, GrainStorage.ChatRoomStore)]
        IPersistentState<ChatRoomGrainState> state)
    {
        _state = state;
        _observers = new ObserverManager<IChatRoomObserver>(TimeSpan.FromHours(1), logger);
    }

    public async Task PostMessage(ChatMessage message)
    {
        _state.State.History.Add(message.ToEntity());
        await UpdateUserOnline(message);
        await _state.WriteStateAsync();
        Console.WriteLine($"{nameof(ChatRoomGrain)} is notifying all observers of the message: {message}");
        await _observers.Notify(x => x.ReceiveMessage(message));
    }

    private async Task UpdateUserOnline(ChatMessage message)
    {
        if (!_volatileState.LastMessageSentByUser.ContainsKey(message.Username))
        {
            _volatileState.LastMessageSentByUser.Add(message.Username, DateTimeOffset.Now);
            await _observers.Notify(o => o.UserOnline(message.Username));
            return;
        }

        _volatileState.LastMessageSentByUser[message.Username] = DateTimeOffset.Now;
    }

    public Task<ReadOnlyCollection<ChatMessage>> Join(IChatRoomObserver observer)
    {
        _observers.Subscribe(observer, observer);
        return Task.FromResult(
            new ReadOnlyCollection<ChatMessage>(_state.State.History.Select(m => m.ToPrimitive()).ToList()));
    }

    public Task Leave(IChatRoomObserver observer)
    {
        _observers.Unsubscribe(observer);
        return Task.CompletedTask;
    }

    public Task<ImmutableArray<ChatMessage>> GetHistory()
    {
        var history = ImmutableArray<ChatMessage>.Empty.AddRange(_state.State.History.Select(m => m.ToPrimitive()));
        return Task.FromResult(history);
    }

    public Task<ImmutableArray<ChatMessage>> GetHistoryPaging(
        int startIndex,
        int numOfMessages,
        GrainCancellationToken requestCancellationToken)
    {
        var result = _state.State.History.Skip(startIndex).Take(numOfMessages).Select(m => m.ToPrimitive())
            .ToImmutableArray();
        return Task.FromResult(result);
    }

    public async Task DeleteMessage(ChatMessage message)
    {
        _state.State.History.Remove(message.ToEntity());
        await _state.WriteStateAsync();
        await _observers.Notify(o => o.DeletedMessage(message));
    }

    public Task<Dictionary<Username, DateTimeOffset>> GetLastMessageSentByUsers()
    {
        return Task.FromResult(_volatileState.LastMessageSentByUser);
    }

    public Task Clear()
    {
        _state.State.History.Clear();
        return _state.WriteStateAsync();
    }
}