using System.Collections.Immutable;
using Chatty.Silo.Primitives;
using Orleans.Concurrency;

namespace Chatty.Silo;

public interface IChatRoomObserver : IGrainObserver
{
    [OneWay]
    Task ReceiveMessage(ChatMessage message);
    
    [OneWay]
    Task DeletedMessage(ChatMessage message);
    
    Task UsersOnlineChanged(ImmutableArray<Username> usersOnline);
}