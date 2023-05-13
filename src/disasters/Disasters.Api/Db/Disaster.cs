using System.ComponentModel.DataAnnotations;

namespace Disasters.Api.Db;

public class Disaster
{
    [Key]
    public Guid DisasterId { get; set; }
    public string Summary { get; set; } = "";
    public Guid LocationId { get; set; }
    public virtual Location Location { get; set; }
}