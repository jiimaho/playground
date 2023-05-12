namespace Disasters.Api;

public class DisastersResponse
{
    public IEnumerable<DisasterResponseItem> Disasters { get; set; }
}

public class DisasterResponseItem
{
    public DateOnly Date { get; set; }

    public string Summary { get; set; }
}