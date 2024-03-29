namespace Disasters.Api.Services;

public class TraceDisasterServiceDecorator(IDisastersService inner, ILogger serilog) : IDisastersService
{
    private readonly ILogger _logger = serilog.ForContext<TraceDisasterServiceDecorator>();
    
    public async Task<IEnumerable<DisasterVm>> GetDisasters()
    {
        _logger.Debug("Tracing disaster service");
        using var _ = Tracing.StartCustomActivity(GetType());
        return await inner.GetDisasters();
    }
}