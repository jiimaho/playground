using Disasters.Api.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
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