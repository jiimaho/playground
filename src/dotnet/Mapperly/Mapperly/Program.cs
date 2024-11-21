// See https://aka.ms/new-console-template for more information

using Mapperly;
using Mapperly.Models;

// Model to dto
var carIncident = new CarIncident
{
    Id = Guid.NewGuid().ToString(),
    CarId = new CarId(Guid.NewGuid()),
    Location = new Location(new Street("Main street"), new City("New York")), 
    Description = "Accident with two persons and a car",
    Severity = Severity.High
};

var mapper = new CarIncidentMapper();
var dto = mapper.MapToDto(carIncident);

Console.WriteLine(dto);

// Dto to model
var dto2 = new CarIncidentDto
{
    Id = Guid.NewGuid().ToString(),
    CarId = new CarId(Guid.NewGuid()),
    Place = new LocationDto { Street = "Main street", City = "New York" },
    Description = "Accident with two persons and a car",
    Severity = "High"
};

var model = mapper.MapToModel(dto2);
Console.WriteLine(model);
