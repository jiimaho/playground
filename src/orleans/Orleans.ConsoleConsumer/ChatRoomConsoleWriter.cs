using System.Collections.Immutable;
using Orleans.Silo;

namespace Orleans.ConsoleConsumer;

public class ChatRoomConsoleWriter : IChatRoomObserver 
{
    public Task ReceiveMessage(ChatMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }

    public Task PeopleOnline(string[] users)
    {
        Console.WriteLine("People online: " + string.Join(", ", users)); 
        return Task.CompletedTask;
    }
    
    public Task UsersOnlineChanged(ImmutableArray<string> usersOnline)
    {
        Console.WriteLine("People online: " + string.Join(", ", usersOnline)); 
        return Task.CompletedTask;
    }
}