using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Orleans.Silo;

[Alias("IChatRoom")]
public interface IChatRoom : IGrainWithStringKey
{
    [Alias("PostMessage")]
    Task PostMessage(ChatMessage chatMessage);
    
    [Alias("Join")]
    Task<ReadOnlyCollection<ChatMessage>> Join(IChatRoomObserver observer);
    
    [Alias("Leave")]
    Task Leave(IChatRoomObserver observer);

    [Alias("GetHistory")]
    Task<ImmutableArray<ChatMessage>> GetHistory();
}