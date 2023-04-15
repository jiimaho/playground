using EventStore.Client;

namespace Todo.Infrastructure.EventStore;

public class EventStoreRepository : IRepository
{
    private readonly EventStoreClient _client;

    public EventStoreRepository()
    {
        var settings = EventStoreClientSettings.Create("esdb://localhost:2113?tls=false");
        _client = new EventStoreClient(settings);
    }

    public async Task<IEnumerable<EventDto>> Get(string stream)
    {
        var result = _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start);

        var events = await result.ToListAsync();

        var evs = events.Select(e => System.Text.Json.JsonSerializer.Deserialize<EventDto>(e.Event.Data.ToArray()));

        return evs;
    }

    public async Task Save(string stream, IEnumerable<EventDto> events)
    {
        var eventDatas = events.Select(e => new EventData(
            Uuid.NewUuid(), 
            "TodoCreatedEvent",
            new ReadOnlyMemory<byte>(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(e)))).ToArray();
        
        await _client.AppendToStreamAsync("todos", StreamState.Any, eventDatas);
    }
}