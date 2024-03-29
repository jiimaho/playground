using System.Diagnostics;

namespace Disasters.Api.Services;

public class ReliefWebDisastersService(
    IHttpClientFactory httpClientFactory,
    ILogger serilog) : IDisastersService
{
    private readonly ILogger _logger = serilog.ForContext<ReliefWebDisastersService>();
    
    public async Task<IEnumerable<DisasterVm>> GetDisasters()
    {
        using var activity = Tracing.DisastersApi.StartActivity(ActivityKind.Client);
        using var client = httpClientFactory.CreateClient(HttpClients.ReliefWeb);

        var response = await client.GetAsync("/v1/disasters?appname=rwint-user-0&profile=list&preset=latest&slim=1");

        response.EnsureSuccessStatusCode();

        activity?.SetTag("Version", "v1");
        activity?.SetTag("StatusCode", response.StatusCode.ToString());
        try
        {
            var content = await response.Content.ReadFromJsonAsync<DisasterResponse>();

            if (content is null)
            {
                return new List<DisasterVm>();
            }
            
            return content.data.Select(x => new DisasterVm(x.fields.name, x.fields.country.First().name));   
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error while parsing response from ReliefWeb API");
            throw;
        }
    }   
}