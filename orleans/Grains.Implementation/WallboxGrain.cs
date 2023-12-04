using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using Orleans.Placement;
using Orleans.Runtime;

namespace Grains.Implementation;

[RandomPlacement]
public class WallboxGrain : Grain, IWallboxGrain, IRemindable
{
    private readonly ILogger<WallboxGrain> _logger;
    private readonly IPersistentState<WallboxState> _state;
    private Task<IGrainReminder> _reminder;

    public WallboxGrain(
        ILogger<WallboxGrain> logger,
        [PersistentState("wallbox", "wallboxStore")] IPersistentState<WallboxState> state)
    {
        _logger = logger;
        _state = state;
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Type} activated with state {State}", nameof(WallboxGrain), _state.State.Status);
        _reminder = this.RegisterOrUpdateReminder("Cleanup", TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return base.OnActivateAsync(cancellationToken);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Type} deactivated with state {State}", nameof(WallboxGrain), _state.State.Status);
        return base.OnDeactivateAsync(reason, cancellationToken);
    }

    public async Task StartCharging(StartChargingDto dto)
    {
        _state.State.Status = "Charging";
        await _state.WriteStateAsync();
        _logger.LogInformation(
            "[{Id}] Charging for user {User} with current {Current}", 
            this.GetPrimaryKey(),
            dto.User.Name,
            dto.Current);
    }
    
    public async Task SayHello(WebSocket webSocket)
    {
        await webSocket.SendAsync(
            "Hej från grain"u8.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);
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