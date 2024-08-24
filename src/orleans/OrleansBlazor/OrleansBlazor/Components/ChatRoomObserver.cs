using Orleans.Silo;

namespace OrleansBlazor.Components;

public class ChatRoomObserver : IChatRoomObserver
{
    private readonly Func<ChatMessage, Task> _invoker;

    public ChatRoomObserver(Func<ChatMessage, Task> invoker)
    {
        _invoker = invoker;
    }

    public Task ReceiveMessage(ChatMessage message) => _invoker(message);
}