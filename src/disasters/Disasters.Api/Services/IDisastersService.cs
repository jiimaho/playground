namespace Disasters.Api.Services;

public interface IDisastersService
{
    Task<IEnumerable<DisasterVm>> GetDisasters();
}