using Disasters.Api.Configuration;
using JetBrains.Annotations;

var logger = LogHelper.CreateStartupLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.AddApplicationServices();

    var app = builder.Build();
    
    app.MapApplicationMiddlewareAndEndpoints();

    await app.RunAsync();
}
catch (Exception e)
{
    logger.Fatal(e, "Application failed unexpectedly");
}

[UsedImplicitly]
public partial class Program;