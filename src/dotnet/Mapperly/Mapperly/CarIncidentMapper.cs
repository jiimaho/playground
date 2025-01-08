using Domain;
using Domain.Models;
using Riok.Mapperly.Abstractions;

namespace Mapperly;

[Mapper]
public partial class CarIncidentMapper
{
    [MapProperty(nameof(CarIncident.Location), nameof(CarIncidentDto.Place))]
    public partial CarIncidentDto MapToDto(CarIncident model);
    
    [MapProperty(nameof(CarIncidentDto.Place), nameof(CarIncident.Location))]
    public partial CarIncident MapToModel(CarIncidentDto dto);
}