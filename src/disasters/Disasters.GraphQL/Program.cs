// See https://aka.ms/new-console-template for more information

using Disasters.GraphQL;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Aspire
    builder.AddServiceDefaults();

    builder.Services.AddGraphQLServer()
        .AddQueryType<Query>();

    var app = builder.Build();

    app.MapGraphQL()
        .AllowAnonymous()
        .WithName("graphql");
    app.MapBananaCakePop()
        .AllowAnonymous();

    Log.Logger.Information("GraphQL Server will start now");
    await app.RunAsync();
}
catch (Exception e) { Log.Fatal(e, "Application failed unexpectedly"); }
finally { Log.CloseAndFlush(); }
