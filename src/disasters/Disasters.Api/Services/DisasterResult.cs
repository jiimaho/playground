using Dunet;

namespace Disasters.Api.Services;

[Union]
public partial record DisasterResult
{
    public partial record Success(IEnumerable<DisasterVm> Disasters);
    public partial record Failure(string Reason);

    public partial record Empty;
}