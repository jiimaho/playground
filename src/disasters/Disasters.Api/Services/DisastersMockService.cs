using System.Diagnostics;

namespace Disasters.Api.Services;

public class DisastersMockService( 
    ILogger logger) : IDisastersService
{
    private readonly ILogger _logger = logger.ForContext<DisastersMockService>();
    
    public Task<IEnumerable<DisasterVm>> GetDisasters()
    {
        using var activity = Tracing.DisastersApi.StartActivity(ActivityKind.Internal);
        
        _logger.Debug("Creating mock disasters");
        IEnumerable<DisasterVm> disasterVms = new List<DisasterVm>
        {
            new("Heavy rain", "Louisiana"),
        };
        _logger.Debug("Returning mock disasters");
        
        return Task.FromResult(disasterVms);
    }
}