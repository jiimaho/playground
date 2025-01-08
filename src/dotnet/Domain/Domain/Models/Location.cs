namespace Domain.Models;

public class Location(Street street, City city) : ValueObject
{
    public Street Street { get; } = street;
    public City City { get; } = city;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
    }

    public override string ToString()
    {
        return $"City is {City} and street is {Street}";
    }
}