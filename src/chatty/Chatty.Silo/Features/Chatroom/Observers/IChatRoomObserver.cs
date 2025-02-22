using Chatty.Silo.Primitives;
using Orleans.Concurrency;

namespace Chatty.Silo.Features.Chatroom.Observers;

[Alias("IChatRoomObserver")]
public interface IChatRoomObserver : IGrainObserver
{
    [OneWay]
    [Alias("ReceiveMessage")]
    Task ReceiveMessage(ChatMessage message);
    
    [OneWay]
    [Alias("DeletedMessage")]
    Task DeletedMessage(ChatMessage message);

    [Alias("UserOnline")]
    Task UserOnline(Username username);
}