using EventStore.Client;

namespace Todo.Infrastructure;

public class Repository
{
    private readonly EventStoreClient _client;

    public Repository()
    {
        var settings = EventStoreClientSettings.Create("esdb://localhost:2113?tls=false");
        _client = new EventStoreClient(settings);
    }

    public async Task<IEnumerable<EventDto>> GetEvents(string stream)
    {
        var result = _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start);

        var events = await result.ToListAsync();

        var evs = events.Select(e => System.Text.Json.JsonSerializer.Deserialize<EventDto>(e.Event.Data.ToArray()));

        return evs;
    }

    public async Task AppendEvents(string stream, IEnumerable<EventDto> events)
    {
        var eventDatas = events.Select(e => new EventData(
            Uuid.NewUuid(), 
            "TodoCreatedEvent",
            new ReadOnlyMemory<byte>(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(e)))).ToArray();
        
        await _client.AppendToStreamAsync("todos", StreamState.Any, eventDatas);
    }
}