using Domain;
using Domain.Models;

namespace Automapper.PrintHelper;

public static class PrintHelper
{
    private static readonly List<ICanPrint> Printers =
    [
        new Spectre1Print()
        // new Spectre2Print()
    ];

    public static void Print(CarIncident model, CarIncidentDto dto)
    {
        foreach (var printer in Printers)
        {
            printer.Print(model, dto);
        }
    }
}