
using Chatty.Silo.Primitives;

namespace Chatty.Silo;

public record ChatRoomVolatileState(Dictionary<Username, DateTimeOffset> LastMessageSentByUser);
