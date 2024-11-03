using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Disasters.Api.Services;

public class DisastersMockService( 
    ILogger logger) : IDisastersService
{
    private readonly ILogger _logger = logger.ForContext<DisastersMockService>();
    
    public Task<DisasterResult> GetDisasters(int? page, int? pageSize)
    {
        using var activity = Trace.DisastersApi.StartActivity(GetType());
        
        _logger.Debug("Creating mock disasters");
        IEnumerable<DisasterVm> disasterVms = new List<DisasterVm>
        {
            new("Heavy rain", "Louisiana"),
        };
        _logger.Debug("Returning mock disasters");

        return Task.FromResult<DisasterResult>(new DisasterResult.Success(disasterVms));
    }

    public Task MarkSafe(MarkSafeVm markSafeVm)
    {
        return Task.CompletedTask;
    }
}