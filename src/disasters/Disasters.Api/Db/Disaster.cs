namespace Disasters.Api.Db;

public class Disaster
{
    public int DisasterId { get; set; }
    public string Summary { get; set; } = "";
    public int LocationId { get; set; }
    public virtual Location Location { get; set; }
}