using AutoMapper;
using Domain;
using Domain.Models;

namespace Automapper.Mapping;

public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<Location, LocationDto>();
        CreateMap<CarIncident, CarIncidentDto>()
            .ForMember(dto => dto.Place, expression => expression.MapFrom(i => i.Location));
    }
}