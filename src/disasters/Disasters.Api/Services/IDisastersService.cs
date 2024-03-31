namespace Disasters.Api.Services;

public interface IDisastersService
{
    Task<IEnumerable<DisasterVm>> GetDisasters(int i, int page);
    
    Task MarkSafe(MarkSafeVm markSafeVm);
}