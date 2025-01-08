using Domain;
using Domain.Models;

namespace Automapper.PrintHelper;

public interface ICanPrint
{
    void Print(CarIncident model, CarIncidentDto dto);
}