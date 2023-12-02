using Microsoft.Extensions.Logging;
using Orleans.Placement;

namespace Grains.Implementation;

[RandomPlacement]
public class StartChargingGrain : Grain, IStartChargingGrain
{
    private readonly ILogger<StartChargingGrain> _logger;
    private readonly IGrainFactory _grainFactory;

    public StartChargingGrain(
        ILogger<StartChargingGrain> logger,
        IGrainFactory grainFactory)
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }

    public Task StartCharging(StartChargingDto dto)
    {
        var wallbox = _grainFactory.GetGrain<IWallboxGrain>(this.GetPrimaryKey());

        if (dto.User.Name != "Jim")
        {
            _logger.LogInformation("Jim is not allowed to start charging");
            return Task.CompletedTask;
        }
        // TODO: Check if wallbox is available
        // TODO: Check if wallbox is not already charging

        return wallbox.StartCharging(dto);
    }
}