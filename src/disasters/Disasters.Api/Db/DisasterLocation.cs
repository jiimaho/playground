namespace Disasters.Api.Db;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class DisasterLocation
{
    public Guid DisasterLocationId { get; set; }
    public Guid DisasterId { get; set; }
    public virtual Disaster Disaster { get; set; }
    public Guid LocationId { get; set; }
    public virtual Location Location { get; set; }
}