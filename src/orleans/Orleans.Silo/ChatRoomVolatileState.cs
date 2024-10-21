using Orleans.Silo.Primitives;

namespace Orleans.Silo;

public record ChatRoomVolatileState(Dictionary<Username, DateTimeOffset> LastMessageSentByUser);
