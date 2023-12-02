namespace Disasters.Api.Disasters;

public class DisastersService(IHttpClientFactory httpClientFactory) : IDisastersService
{
    public async Task<IEnumerable<DisasterVm>> GetDisasters()
    {
        using var client = httpClientFactory.CreateClient();

        var response = await client.GetAsync("https://api.reliefweb.int/v1/disasters?appname=rwint-user-0&profile=list&preset=latest&slim=1");

        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadFromJsonAsync<DisasterResponse>();

        if (content is null)
            throw new Exception("Failed to deserialize response");

        return content.data.Select(x => new DisasterVm(x.fields.name));
    }   
}