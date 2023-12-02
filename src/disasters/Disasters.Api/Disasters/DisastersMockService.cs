using Serilog;

namespace Disasters.Api.Disasters;

public class DisastersMockService(ILogger<DisastersMockService> Logger, Serilog.ILogger SeriLogger) : IDisastersService
{
    public Task<IEnumerable<DisasterVm>> GetDisasters()
    {
        Logger.LogInformation(
            "{SourceContext} Getting disasters using Microsoft logger {Time:yyyy-MM-dd:HH:mm:ss} {Person:l} {Amount:NN.NN}", 
            GetType().Name,
            DateTimeOffset.Now,
            "Jim",
            100.456m);
        SeriLogger.Information(
            "{SourceContext} Getting disasters using Serilog logger {Time:yyyy-MM-dd:HH:mm:ss} {Person:l} {Amount:NN.NN}", 
            GetType().Name,
            DateTimeOffset.Now,
            "Jim",
            100.456m);
            
        
        IEnumerable<DisasterVm> disasterVms = new List<DisasterVm> { new("DisasterMock") };
        
        return Task.FromResult(disasterVms);
    }
}