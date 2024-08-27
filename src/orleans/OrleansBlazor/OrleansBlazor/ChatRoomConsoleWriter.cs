using Orleans.Silo;

namespace OrleansBlazor;

public class ChatRoomConsoleWriter : IChatRoomObserver 
{
    public Task ReceiveMessage(ChatMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}