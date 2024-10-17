using Orleans.Silo;

namespace ConsoleApp1;

public class ChatRoomConsoleWriter : IChatRoomObserver 
{
    public Task ReceiveMessage(ChatMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}