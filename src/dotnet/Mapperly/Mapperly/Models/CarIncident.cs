namespace Mapperly.Models;

public class CarIncident : ValueObject
{
    public required string Id { get; init; }
    public required CarId CarId { get; init; }
    public required Location Location { get; init; }
    public required string Description { get; init; }
    
    public required Severity Severity { get; init; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}