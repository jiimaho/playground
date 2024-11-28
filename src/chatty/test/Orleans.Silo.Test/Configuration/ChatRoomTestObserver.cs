using System.Collections.ObjectModel;
using Chatty.Silo;
using Chatty.Silo.Primitives;

namespace Orleans.Silo.Test.Configuration;

public class ChatRoomTestObserver : IChatRoomObserver
{
    public Task ReceiveMessage(ChatMessage message)
    {
        _receivedMessages.Add(message);
        return Task.CompletedTask;
    }

    public Task DeletedMessage(ChatMessage message)
    {
        return Task.CompletedTask;
    }

    public Task UserOnline(Username username)
    {
        return Task.CompletedTask;
    }

    private List<ChatMessage> _receivedMessages { get; } = new();
    public ReadOnlyCollection<ChatMessage> ReceivedMessages => _receivedMessages.AsReadOnly();
}