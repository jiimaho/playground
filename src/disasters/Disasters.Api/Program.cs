using Disasters.Api.Configuration;
using Disasters.Api.Endpoints;
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
    builder.AddServiceDefaults();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "Handled {RequestPath} {Person} {Properties}";
    });

    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.MapDisasters();

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