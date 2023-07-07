using Disasters.Actors;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .CreateLogger();

var builder = Host.CreateDefaultBuilder(args);
    
var host = builder
    .ConfigureServices(services =>
    {
        services.AddSerilog(logger);
        services.AddHostedService<AkkaService>();
    })
    .Build();

await host.RunAsync();