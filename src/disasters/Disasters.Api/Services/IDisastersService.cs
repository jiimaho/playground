namespace Disasters.Api.Services;

public interface IDisastersService
{
    Task<DisasterResult> GetDisasters(int? page, int? pageSize);
    
    Task MarkSafe(MarkSafeVm markSafeVm);
}