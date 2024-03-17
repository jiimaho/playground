namespace Disasters.Api.Services;

public class DisastersMockService(
    ILogger<DisastersMockService> microsoftLogger, 
    ILogger logger,
    TimeProvider timeProvider) : IDisastersService
{
    public Task<IEnumerable<DisasterVm>> GetDisasters()
    {
        logger.ForContext(GetType()).Information("Getting mock disasters using Serilog logger");
        
        IEnumerable<DisasterVm> disasterVms = new List<DisasterVm>
        {
            new("Heavy rain", "Louisiana"),
        };
        
        return Task.FromResult(disasterVms);
    }
}