using System.Text;
using Domain.Models;

namespace Domain;

public class CarIncidentDto
{
    public string Id { get; set; }
    public CarId CarId { get; set; } // A incident for now is simple and always contains only one car
    public LocationDto Place { get; set; }
    public string Description { get; set; }
    
    public string Severity { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("----");
        sb.AppendLine($"Id: {Id}");
        sb.AppendLine($"CarId: {CarId}");
        sb.AppendLine($"Location: {Place}");
        sb.AppendLine($"Description: {Description}");
        sb.AppendLine($"Severity: {Severity}");
        sb.AppendLine("----");
        return sb.ToString();
    }
}

public class LocationDto
{
    public string Street { get; set; }
    public string City { get; set; }

    public override string ToString()
    {
        return $"City is {City} and street is {Street}";
    }
}