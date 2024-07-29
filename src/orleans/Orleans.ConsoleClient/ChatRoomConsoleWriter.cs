using Orleans.Silo;

namespace ConsoleApp1;

public class ChatRoomConsoleWriter : IChatRoomObserver 
{
    public Task ReceiveMessage(ChatMessage message)
    {
        Console.WriteLine($"Receive message: {message.Message} from user {message.User}");
        return Task.CompletedTask;
    }
}