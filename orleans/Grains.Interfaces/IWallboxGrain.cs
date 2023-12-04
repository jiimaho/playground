using System.Net.WebSockets;

namespace Grains;

public interface IWallboxGrain : IGrainWithGuidKey
{
    Task StartCharging(StartChargingDto dto);

    Task SayHello(WebSocket webSocket);
}