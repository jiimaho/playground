
using Chatty.Silo.Primitives;

namespace Chatty.Silo.Features.Chatroom.Grains;

public record ChatRoomVolatileState(Dictionary<Username, DateTimeOffset> LastMessageSentByUser);
