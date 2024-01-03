namespace Grains;

public interface IWallboxGrain : IGrainWithGuidKey
{
    Task StartCharging(StartChargingDto dto);
}