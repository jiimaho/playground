using System.Collections.Immutable;
using Orleans.Silo;

namespace OrleansBlazor.Components;

public class ChatRoomObserver : IChatRoomObserver
{
    private readonly Func<ChatMessage, Task> _invoker;
    private readonly Func<ImmutableArray<string>, Task> _usersOnlineChangedInvoker;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public ChatRoomObserver(
        Func<ChatMessage, Task> invoker,
        Func<ImmutableArray<string>, Task> usersOnlineChangedInvoker)
    {
        _invoker = invoker;
        _usersOnlineChangedInvoker = usersOnlineChangedInvoker;
    }

    public Task ReceiveMessage(ChatMessage message) => _invoker(message);

    public Task UsersOnlineChanged(ImmutableArray<string> usersOnline) =>
        _usersOnlineChangedInvoker.Invoke(usersOnline);
}