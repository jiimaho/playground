using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Orleans.Silo.Primitives;

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

    [Alias("GetLastMessageSentByUsers")]
    Task<Dictionary<Username, DateTimeOffset>> GetLastMessageSentByUsers();

    [Alias("Clear")]
    Task Clear();

    [Alias("GetHistoryPaging")]
    Task<ImmutableArray<ChatMessage>> GetHistoryPaging(
        int startIndex,
        int numOfMessages,
        GrainCancellationToken requestCancellationToken);
}