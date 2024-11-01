using Newtonsoft.Json;
using Orleans.Storage;

namespace Orleans.Silo.Configuration;

public class CustomGrainStorageSerialization : IGrainStorageSerializer 
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new();

    public BinaryData Serialize<T>(T input)
    {
        var json = JsonConvert.SerializeObject(input, typeof(T), _jsonSerializerSettings);
        return new BinaryData(json);
    }

    public T Deserialize<T>(BinaryData input)
    {
        var obj = JsonConvert.DeserializeObject<T>(input.ToString());

        if (obj is ChatRoomState state)
        {
            foreach (var chatMessage in state.History)
            {
                if (string.IsNullOrWhiteSpace(chatMessage.ChatRoomId))
                {
                    chatMessage.ChatRoomId = "all";
                }
            }
        }
        
        return obj!;
    }
}