using Domain;
using Domain.Models;
using Spectre.Console;

namespace Automapper.PrintHelper;

public class Spectre2Print : ICanPrint
{
    public void Print(CarIncident model, CarIncidentDto dto)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[bold yellow]Property[/]");
        table.AddColumn("[bold green]CarIncident[/]");
        table.AddColumn("[bold blue]CarIncidentDto[/]");

        table.AddRow("Id", model.Id, dto.Id);
        table.AddRow("CarId", model.CarId.ToString(), dto.CarId.ToString());
        table.AddRow("Location", model.Location.ToString(), dto.Place.ToString());
        table.AddRow("Description", model.Description, dto.Description);
        table.AddRow("Severity", model.Severity.ToString(), dto.Severity);

        AnsiConsole.Write(new Panel(table).Header("Comparison of CarIncident and CarIncidentDto", Justify.Center).BorderStyle(Style.Parse("green")));

    }
}