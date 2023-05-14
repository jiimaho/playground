namespace Disasters.Api;

public record DisasterRequest
{
    public DateTimeOffset Occured { get; set; }
    public string Summary { get; set; } = "";
    public IEnumerable<LocationRequest> Locations { get; set; } = new List<LocationRequest>();
}