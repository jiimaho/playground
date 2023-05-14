using System.ComponentModel.DataAnnotations;

namespace Disasters.Api.Db;

public class Disaster
{
    [Key]
    public Guid DisasterId { get; set; }

    public DateTimeOffset Occured { get; set; }
    public string Summary { get; set; } = "";
    public virtual List<Location> Locations { get; set; }
}