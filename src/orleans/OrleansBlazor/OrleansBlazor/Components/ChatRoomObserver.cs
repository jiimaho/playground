using System.Collections.Immutable;
using Orleans.Silo;
using Orleans.Silo.Primitives;

namespace OrleansBlazor.Components;

public class ChatRoomObserver : IChatRoomObserver
{
    private readonly Func<ChatMessage, Task> _invoker;
    private readonly Func<ImmutableArray<Username>, Task> _usersOnlineChangedInvoker;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public ChatRoomObserver(
        Func<ChatMessage, Task> invoker,
        Func<ImmutableArray<Username>, Task> usersOnlineChangedInvoker)
    {
        _invoker = invoker;
        _usersOnlineChangedInvoker = usersOnlineChangedInvoker;
    }

    public Task ReceiveMessage(ChatMessage message) => _invoker(message);

    public Task UsersOnlineChanged(ImmutableArray<Username> usersOnline) =>
        _usersOnlineChangedInvoker.Invoke(usersOnline);
}