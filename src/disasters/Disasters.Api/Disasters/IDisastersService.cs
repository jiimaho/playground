namespace Disasters.Api.Disasters;

public interface IDisastersService
{
    Task<IEnumerable<DisasterVm>> GetDisasters();
}