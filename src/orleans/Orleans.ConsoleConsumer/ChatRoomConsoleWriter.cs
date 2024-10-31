using System.Collections.Immutable;
using Orleans.Silo;
using Orleans.Silo.Primitives;

namespace Orleans.ConsoleConsumer;

public class ChatRoomConsoleWriter : IChatRoomObserver 
{
    public Task ReceiveMessage(ChatMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }

    public Task DeletedMessage(ChatMessage message)
    {
        Console.WriteLine($"Message deleted: {message}");
        return Task.CompletedTask;
    }

    public Task PeopleOnline(string[] users)
    {
        Console.WriteLine("People online: " + string.Join(", ", users)); 
        return Task.CompletedTask;
    }
    
    public Task UsersOnlineChanged(ImmutableArray<Username> usersOnline)
    {
        Console.WriteLine("People online: " + string.Join(", ", usersOnline)); 
        return Task.CompletedTask;
    }
}