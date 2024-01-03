using Grains;
using Orleans.Runtime;
using Orleans.Streams;

namespace Orleans.Silo.Web;

public class StatusListenerBackgroundService : IHostedService
{
    private readonly IClusterClient _clusterClient;
    private StreamSubscriptionHandle<WallboxStatusEvent> _handle;

    public StatusListenerBackgroundService(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var stream = _clusterClient.GetStreamProvider("in-memory")
            .GetStream<WallboxStatusEvent>(StreamId.Create("wallbox-status", "1"));

        _handle = await stream.SubscribeAsync(async (message, token) =>
        {
            Console.WriteLine(message.ToString());
        });
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _handle.UnsubscribeAsync();
    }
}