using Domain;
using Domain.Models;
using Spectre.Console;

namespace Automapper.PrintHelper;

public class Spectre1Print : ICanPrint
{
    public void Print(CarIncident model, CarIncidentDto dto)
    {
        var table = new Table();
        table.AddColumn("Property");
        table.AddColumn("CarIncident");
        table.AddColumn("CarIncidentDto");

        table.AddRow("Id", model.Id, dto.Id);
        table.AddRow("CarId", model.CarId.ToString(), dto.CarId.ToString());
        table.AddRow("Location", model.Location.ToString(), dto.Place.ToString());
        table.AddRow("Description", model.Description, dto.Description);
        table.AddRow("Severity", model.Severity.ToString(), dto.Severity);

        AnsiConsole.Write(table);
    }
}