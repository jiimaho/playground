﻿// See https://aka.ms/new-console-template for more information

using Disasters.GraphQL.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Debug()
    .WriteTo.Console(
        theme: AnsiConsoleTheme.Code, 
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext} {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddApplicationServices();

    var app = builder.Build();

    app.MapApplicationEndpoints();

    Log.Logger.Information("GraphQL Server will start now");
    await app.RunAsync();
}
catch (Exception e)
{
    logger.Fatal(e, "Application failed unexpectedly");
}
