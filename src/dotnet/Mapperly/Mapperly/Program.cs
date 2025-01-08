using Domain;
using Domain.Models;
using Mapperly;

// Mapper
var mapper = new CarIncidentMapper();

// Model 
var carIncident = new CarIncident
{
    Id = Guid.NewGuid().ToString(),
    CarId = new CarId(Guid.NewGuid()),
    Location = new Location(new Street("Main street"), new City("New York")), 
    Description = "Accident with two persons and a car",
    Severity = Severity.High
};

// Map to dto 
var dto = mapper.MapToDto(carIncident);

Console.WriteLine($"Model id: {carIncident.Id}                  | Dto id: {dto.Id}");
Console.WriteLine($"Model car id: {carIncident.CarId}           | Dto car id: {dto.CarId}");
Console.WriteLine($"Model location: {carIncident.Location}      | Dto location: {dto.Place}");
Console.WriteLine($"Model description: {carIncident.Description} | Dto description: {dto.Description}");
Console.WriteLine($"Model severity: {carIncident.Severity}      | Dto severity: {dto.Severity}");


// Map to model
var dto2 = new CarIncidentDto
{
    Id = Guid.NewGuid().ToString(),
    CarId = new CarId(Guid.NewGuid()),
    Place = new LocationDto { Street = "Main street", City = "New York" },
    Description = "Accident with two persons and a car",
    Severity = "High"
};

var model = mapper.MapToModel(dto2);

Console.WriteLine($"Dto id: {dto2.Id}                  | Model id: {model.Id}");
Console.WriteLine($"Dto car id: {dto2.CarId}           | Model car id: {model.CarId}");
Console.WriteLine($"Dto location: {dto2.Place}         | Model location: {model.Location}");
Console.WriteLine($"Dto description: {dto2.Description} | Model description: {model.Description}");
Console.WriteLine($"Dto severity: {dto2.Severity}      | Model severity: {model.Severity}");

Console.WriteLine("End of program");