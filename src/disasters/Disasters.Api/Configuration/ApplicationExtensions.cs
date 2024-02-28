using Disasters.Api.Authentication;
using Disasters.Api.Authorization;
using Disasters.Api.Disasters;
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
            builder.Services.AddSingleton<IDisastersService, DisastersMockService>();
        }
        else
        {
            builder.Services.AddSingleton<IDisastersService, DisastersService>();   
        }

        return builder;
    } 
}