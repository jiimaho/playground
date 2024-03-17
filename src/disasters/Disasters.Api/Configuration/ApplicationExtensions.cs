using Disasters.Api.Configuration.Authentication;
using Disasters.Api.Configuration.Authorization;
using Disasters.Api.Services;
using Serilog;

namespace Disasters.Api.Configuration;

public static class ApplicationExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog();
        builder.Services.AddSerilog(Log.Logger);
    
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton(TimeProvider.System);
        
        builder.AddApplicationAuthentication();
        builder.AppApplicationAuthorization();
        
        if (builder.Environment.IsDevelopment())
        {
            Log.Logger.Information("Using {DisasterService}", nameof(DisastersMockService));
            builder.Services.AddSingleton<IDisastersService, DisastersMockService>();
        }
        else
        {
            Log.Logger.Information("Using {DisasterService}", nameof(DisastersService));
            builder.Services.AddSingleton<IDisastersService, DisastersService>();   
        }

        return builder;
    } 
}