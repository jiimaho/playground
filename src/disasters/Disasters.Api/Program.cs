using Disasters.Api.Configuration;
using JetBrains.Annotations;
using Serilog;

Log.Logger = LogHelper.Bootstrap();

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
    Log.Fatal(e, "Application failed unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

[UsedImplicitly]
public partial class Program;