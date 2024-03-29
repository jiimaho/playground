using Disasters.Api.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Disasters", LogEventLevel.Debug)
    .MinimumLevel.Warning()
    .WriteTo.Console(
        theme: AnsiConsoleTheme.Code, 
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Logger.Information("Starting up");
    var builder = WebApplication.CreateBuilder(args);

    builder.AddApplicationServices();

    var app = builder.Build();

    app.MapApplicationMiddlewareAndEndpoints();

    await app.RunAsync();
}
catch (Exception e)
{
    Log.Fatal(e, "Application failed unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}