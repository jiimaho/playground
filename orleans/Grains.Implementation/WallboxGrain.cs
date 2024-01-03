using Microsoft.Extensions.Logging;
using Orleans.Placement;
using Orleans.Runtime;
using Orleans.Streams;

namespace Grains.Implementation;

[RandomPlacement]
public class WallboxGrain : Grain, IWallboxGrain, IRemindable
{
    private readonly ILogger<WallboxGrain> _logger;
    private readonly IPersistentState<WallboxState> _state;
    private IDisposable? _statusUpdateTimer;
    private IAsyncStream<WallboxStatusEvent>? _statusStream;

    public WallboxGrain(
        ILogger<WallboxGrain> logger,
        [PersistentState("wallbox", "wallboxStore")] IPersistentState<WallboxState> state)
    {
        _logger = logger;
        _state = state;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Type} activated with state {State}", nameof(WallboxGrain), _state.State.Status);
        
        GetStatusStream();

        RegisterStatusUpdateTimer();
        
        await PublishActivated();
        
        await base.OnActivateAsync(cancellationToken);
    }

    private Task PublishActivated() => 
        _statusStream?.OnNextAsync(new WallboxStatusEvent("activated and fresh")) ?? Task.CompletedTask;

    private void RegisterStatusUpdateTimer()
    {
        _statusUpdateTimer = RegisterTimer(async _ =>
        {
            _state.State.Status = (WallboxStatus) new Random().Next(0, Enum.GetNames<WallboxStatus>().Length);
            await _state.WriteStateAsync();
            await _statusStream?.OnNextAsync(new WallboxStatusEvent($"{_state.State.Status} {DateTimeOffset.Now}"))!;
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }

    private void GetStatusStream()
    {
        var streamProvider = this.GetStreamProvider("in-memory");
        var wallboxStatus = StreamId.Create("wallbox-status", "1");
        _statusStream = streamProvider.GetStream<WallboxStatusEvent>(wallboxStatus);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Type} deactivated with state {State}", nameof(WallboxGrain), _state.State.Status);
        _statusUpdateTimer?.Dispose();
        return base.OnDeactivateAsync(reason, cancellationToken);
    }

    public async Task StartCharging(StartChargingDto dto)
    {
        _state.State.Status = WallboxStatus.Charging;
        await _state.WriteStateAsync();
        _logger.LogInformation(
            "[{Id}] [{TraceId}] Charging for user {User} with current {Current}", 
            this.GetPrimaryKey(),
            RequestContext.Get("TraceId") as string ?? "",
            dto.User.Name,
            dto.Current);
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