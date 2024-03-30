using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Disasters.Api.Configuration;

public static class LogHelper
{
    private const string MessageTemplate =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj} {Properties:j}{NewLine}{Exception}"; 
    
    public static LoggerConfiguration StandardConsole(this LoggerSinkConfiguration sinkConfiguration) => 
        sinkConfiguration.Console(theme: AnsiConsoleTheme.Code, outputTemplate: MessageTemplate);
    
    public static ILogger Bootstrap() => new LoggerConfiguration()
        .MinimumLevel.Override("Disasters", LogEventLevel.Debug)
        .MinimumLevel.Warning()
        .WriteTo.StandardConsole()
        .CreateBootstrapLogger();
}