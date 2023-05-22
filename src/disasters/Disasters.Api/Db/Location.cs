using System.ComponentModel.DataAnnotations;

namespace Disasters.Api.Db;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Location
{
    [Key]
    public Guid LocationId { get; set; }

    public string Country { get; set; } = "";

    public virtual List<DisasterLocation> DisasterLocations { get; set; }
}