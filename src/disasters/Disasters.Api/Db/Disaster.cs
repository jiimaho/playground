using System.ComponentModel.DataAnnotations;

namespace Disasters.Api.Db;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Disaster
{
    [Key]
    public Guid DisasterId { get; set; }

    public DateTimeOffset Occured { get; set; }
    public string Summary { get; set; } = "";
    public virtual List<DisasterLocation> DisasterLocations { get; set; }
}