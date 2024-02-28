namespace Disasters.Api.Disasters;

public class DisastersMockService(
    ILogger<DisastersMockService> microsoftLogger, 
    ILogger logger,
    TimeProvider timeProvider) : IDisastersService
{
    public Task<IEnumerable<DisasterVm>> GetDisasters()
    {
        microsoftLogger.LogInformation(
            "{SourceContext} Getting disasters using Microsoft logger {Time:yyyy-MM-dd:HH:mm:ss} {Person:l} {Amount:NN.NN}", 
            GetType().Name,
            timeProvider.GetUtcNow(),
            "Jim",
            100.456m);
        logger.Information(
            "{SourceContext} Getting disasters using Serilog logger {Time:yyyy-MM-dd:HH:mm:ss} {Person:l} {Amount:NN.NN}", 
            GetType().Name,
            timeProvider.GetUtcNow(),
            "Jim",
            100.456m);
        
        IEnumerable<DisasterVm> disasterVms = new List<DisasterVm> { new("Heavy rain in louisiana") };
        
        return Task.FromResult(disasterVms);
    }
}