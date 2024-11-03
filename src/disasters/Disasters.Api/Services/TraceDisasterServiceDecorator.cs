namespace Disasters.Api.Services;

public class TraceDisasterServiceDecorator(IDisastersService inner, ILogger serilog) : IDisastersService
{
    private readonly ILogger _logger = serilog.ForContext<TraceDisasterServiceDecorator>();
    
    public async Task<DisasterResult> GetDisasters(int? page, int? pageSize)
    {
        _logger.Debug("Tracing disaster service");
        using var _ = Trace.DisastersApi.StartActivity(GetType());
        return await inner.GetDisasters(page, pageSize);
    }

    public async Task MarkSafe(MarkSafeVm markSafeVm)
    {
        _logger.Debug("Tracing disaster service");
        using var _ = Trace.DisastersApi.StartActivity(GetType());
        await inner.MarkSafe(markSafeVm);
    }
}