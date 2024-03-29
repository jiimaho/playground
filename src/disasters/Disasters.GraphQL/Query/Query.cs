using JetBrains.Annotations;

namespace Disasters.GraphQL.Query;

public class Query
{
    [PublicAPI]
    public async Task<IEnumerable<Disaster>> GetDisasters(
        [Service] IHttpClientFactory factory,
        [Service] IConfiguration configuration,
        [Service] ILogger serilog)
    {
        var logger = serilog.ForContext(GetType());
        var client = factory.CreateClient("backend");
        logger.Debug("Created client and will call the backend API");
        var response = await client.GetAsync("/disasters");
        response.EnsureSuccessStatusCode();

        try
        {
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<Disaster>>();
            if (data is null)
            {
                throw new Exception("No data found!");
            }
            logger
                .ForContext("ResponseData", response.Content.ReadAsStringAsync())
                .Debug("Data received from the backend API");
            return data;
        }
        catch (Exception e)
        {
            logger.Error(e, "Error while reading data from the backend API");
            throw;
        }
    }
}