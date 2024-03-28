using JetBrains.Annotations;

namespace Disasters.GraphQL.Query;

public class Query
{
    [PublicAPI]
    public async Task<IEnumerable<Disaster>> GetDisasters(
        [Service] IHttpClientFactory factory,
        [Service] IConfiguration configuration)
    {
        using (var activity = Tracing.DisastersGraphQl.StartActivity("About to get..."))
        {
            activity?.SetTag("ThreadId", Thread.CurrentThread.ManagedThreadId);
            await Task.Delay(2000);
        }
        
        var client = factory.CreateClient("backend");
        var response = await client.GetAsync("/disasters");
        response.EnsureSuccessStatusCode();

        try
        {
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<Disaster>>();
            if (data is null) throw new Exception("No data found!");
            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}