using JetBrains.Annotations;

namespace Disasters.GraphQL;

public class Query
{
    [PublicAPI]
    public async Task<IEnumerable<Disaster>> GetDisasters(
        [Service] IHttpClientFactory factory,
        [Service] IConfiguration configuration)
    {
        var client = factory.CreateClient("DisastersBackend");

        // var host = configuration.GetValue<string>("disastersbackend");
        // var response = await client.GetFromJsonAsync<IEnumerable<Disaster>>($"http://{host}/disasters");
        var response = await client.GetAsync($"http://_disasters.backend/disasters");
        response.EnsureSuccessStatusCode();

        try
        {
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<Disaster>>();
            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}