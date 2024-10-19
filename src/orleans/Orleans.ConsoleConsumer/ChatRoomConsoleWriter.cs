using Orleans.Silo;

namespace Orleans.ConsoleConsumer;

public class ChatRoomConsoleWriter : IChatRoomObserver 
{
    public Task ReceiveMessage(ChatMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}