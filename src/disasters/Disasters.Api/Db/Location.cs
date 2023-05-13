using System.ComponentModel.DataAnnotations;

namespace Disasters.Api.Db;

public class Location
{
    [Key]
    public Guid LocationId { get; set; }

    public string Country { get; set; } = "";
}