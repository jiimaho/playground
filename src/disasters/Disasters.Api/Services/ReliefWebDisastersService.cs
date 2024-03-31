using System.Diagnostics;

namespace Disasters.Api.Services;

public class ReliefWebDisastersService(
    IHttpClientFactory httpClientFactory,
    ILogger serilog) : IDisastersService
{
    private readonly ILogger _logger = serilog.ForContext<ReliefWebDisastersService>();
    
    public async Task<IEnumerable<DisasterVm>> GetDisasters(int page, int pageSize)
    {
        using var a = Trace.DisastersApi.StartActivity(GetType());
        if (a is null)
        {
            _logger.Debug("No activity found, skipping tracing");
        }
        _logger.Debug("Calling ReliefWeb API to get disasters");
        using var client = httpClientFactory.CreateClient(HttpClients.ReliefWeb);

        var response = await client.GetAsync("/v1/disasters?appname=rwint-user-0&profile=list&preset=latest&slim=1");

        response.EnsureSuccessStatusCode();

        Activity.Current?.SetTag("RawResponse", await response.Content.ReadAsStringAsync());
        try
        {
            var content = await response.Content.ReadFromJsonAsync<ReliefWebDisasterResponse>();

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

    public Task MarkSafe(MarkSafeVm markSafeVm)
    {
        return Task.CompletedTask;
    }
}