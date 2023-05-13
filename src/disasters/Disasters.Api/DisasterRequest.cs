namespace Disasters.Api;

public record DisasterRequest
{
    public DateTimeOffset Occured { get; set; }
    public string Summary { get; set; } = "";
    public LocationRequest Location { get; set; } = new LocationRequest();
}