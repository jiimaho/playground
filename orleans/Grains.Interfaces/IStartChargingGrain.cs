namespace Grains;

public interface IStartChargingGrain : IGrainWithGuidKey
{
    [ResponseTimeout("00:00:05")]
    Task StartCharging(StartChargingDto dto);
}