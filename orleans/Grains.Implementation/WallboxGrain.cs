using Microsoft.Extensions.Logging;
using Orleans.Placement;
using Orleans.Runtime;

namespace Grains.Implementation;

[RandomPlacement]
public class WallboxGrain : Grain, IWallboxGrain, IRemindable
{
    private readonly ILogger<WallboxGrain> _logger;
    private Task<IGrainReminder> _reminder;

    public WallboxGrain(ILogger<WallboxGrain> logger)
    {
        _logger = logger;
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Activated");
        _reminder = this.RegisterOrUpdateReminder("Cleanup", TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return base.OnActivateAsync(cancellationToken);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deactivated. OnDeactivateAsync is not guaranteed to get called in all situations, for example, in case of a server failure or other abnormal event");
        return base.OnDeactivateAsync(reason, cancellationToken);
    }

    public Task StartCharging(StartChargingDto dto)
    {
        _logger.LogInformation(
            "[{Id}] Charging for user {User} with current {Current}", 
            this.GetPrimaryKey(),
            dto.User.Name,
            dto.Current);
        return Task.CompletedTask;
    }

    public Task ReceiveReminder(string reminderName, TickStatus status)
    {
        if (reminderName != "Cleanup")
        {
            _logger.LogInformation("Received wrong reminder {ReminderName}", reminderName);
            return Task.CompletedTask;
        }
        _logger.LogInformation("[{Time:mm:ss}] [{Wallbox}] Cleaning up some resources that could be stuck or so", DateTimeOffset.Now, this.GetPrimaryKey());
        return Task.CompletedTask;
    }
}