using JetBrains.Annotations;

namespace Chatty.Silo.Features.Chatroom.Grains;

// Keep free from complex types. Serialization must be easy.
[UsedImplicitly]
public record ChatRoomGrainState
{
    public List<ChatMessageEntity> History { get; } = [];
};