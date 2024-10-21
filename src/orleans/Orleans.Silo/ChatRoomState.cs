using JetBrains.Annotations;
using Orleans.Silo.Primitives;

namespace Orleans.Silo;

[UsedImplicitly]
public record ChatRoomState
{
    public List<ChatMessage> History { get; } = [];
};