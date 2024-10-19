namespace Orleans.Silo;

public record ChatRoomVolatileState(Dictionary<string, DateTimeOffset> LastMessageSentByUser);
