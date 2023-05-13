namespace Disasters.Api;

public record DisasterRequest
{
    public DateTimeOffset Time { get; set; }
    public string Summary { get; set; }
}