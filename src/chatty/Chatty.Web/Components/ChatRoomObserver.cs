using Chatty.Silo.Features.Chatroom.Observers;
using Chatty.Silo.Primitives;

namespace Chatty.Web.Components;

public class ChatRoomObserver : IChatRoomObserver
{
    private readonly Func<ChatMessage, Task> _receivedMessageCallback;
    private readonly Func<ChatMessage, Task> _deletedMessageCallback;
    private readonly Func<Username, Task> _usersOnlineChangedInvoker;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ChatRoomObserver(
        Func<ChatMessage, Task> receivedMessageCallback, 
        Func<ChatMessage, Task> deletedMessageCallback,
        Func<Username, Task> usersOnlineChangedInvoker)
    {
        _receivedMessageCallback = receivedMessageCallback;
        _deletedMessageCallback = deletedMessageCallback;
        _usersOnlineChangedInvoker = usersOnlineChangedInvoker;
    }

    public Task ReceiveMessage(ChatMessage message) => _receivedMessageCallback(message);

    public Task DeletedMessage(ChatMessage message) => _deletedMessageCallback(message);

    public Task UserOnline(Username usersOnline) =>
        _usersOnlineChangedInvoker.Invoke(usersOnline);
}