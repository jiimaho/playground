using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Chatty.Silo.Features.Chatroom.Observers;
using Chatty.Silo.Primitives;

namespace Chatty.Silo.Features.Chatroom.Grains;

[Alias("IChatRoom")]
public interface IChatRoom : IGrainWithStringKey
{
    [Alias("PostMessage")]
    Task PostMessage(ChatMessage message);

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
    Task<PagingResult<ChatMessage>> GetHistoryPaging(
        int page,
        int pageSize,
        GrainCancellationToken requestCancellationToken);

    [Alias("DeleteMessage")]
    Task DeleteMessage(ChatMessage message);
}