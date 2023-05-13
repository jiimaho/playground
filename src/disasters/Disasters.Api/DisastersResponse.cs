namespace Disasters.Api;

public record DisastersResponse
{
    public IEnumerable<DisasterResponseItem> Disasters { get; set; } = new List<DisasterResponseItem>();
}

public record DisasterResponseItem
{
    public Guid DisasterId { get; set; }
    public DateTimeOffset Occured { get; set; }
    public string Summary { get; set; } = "";
    public LocationResponseItem Location { get; set; } = new();
}

public record LocationResponseItem
{
    public Guid LocationId { get; set; }
    public string Country { get; set; } = "";
}