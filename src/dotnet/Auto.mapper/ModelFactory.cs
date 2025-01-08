using AutoBogus;
using Domain.Models;

namespace Automapper;

public static class ModelFactory
{
    public static CarIncident GenerateCarIncident()
    {
        var cityFaker = new AutoFaker<City>()
            .CustomInstantiator(faker => new City(faker.Address.City()));
        var streetFaker = new AutoFaker<Street>()
            .CustomInstantiator(faker => new Street(faker.Address.StreetAddress()));
        var locationFaker = new AutoFaker<Location>()
            .CustomInstantiator(_ => new Location(streetFaker.Generate(), cityFaker.Generate()));
        var severityFaker = new AutoFaker<Severity>()
            .CustomInstantiator(faker => faker.PickRandomParam(Severity.Low, Severity.Medium, Severity.High));
        var model = new AutoFaker<CarIncident>()
            .RuleFor(p => p.Id, faker => faker.Random.Guid().ToString())
            .RuleFor(p => p.Location, () => locationFaker.Generate())
            .RuleFor(p => p.Description, faker => faker.Lorem.Sentence())
            .RuleFor(p => p.Severity, () => severityFaker.Generate()).Generate();

        return model;
    }
}