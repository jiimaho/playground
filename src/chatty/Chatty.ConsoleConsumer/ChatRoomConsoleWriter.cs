using System.Collections.Immutable;
using Chatty.Silo;
using Chatty.Silo.Primitives;

namespace Chatty.ConsoleConsumer;

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
    
    public Task UserOnline(Username username)
    {
        Console.WriteLine("People online: " + string.Join(", ", username)); 
        return Task.CompletedTask;
    }
}