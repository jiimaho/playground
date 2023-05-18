namespace Disasters.Api.Db;

public class DisasterLocation
{
    public Guid DisasterLocationId { get; set; }
    public virtual Disaster Disaster { get; set; }
    public virtual Location Location { get; set; }
}