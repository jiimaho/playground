using Disasters.Api.Configuration.Authentication;
using Disasters.Api.Configuration.Authorization;
using Disasters.Api.Endpoints;
using Disasters.Api.Middleware;
using Disasters.Api.Services;
using Serilog;

namespace Disasters.Api.Configuration;

public static class ApplicationExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource("Disasters.Api"));
        
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(Log.Logger);
    
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton(TimeProvider.System);
        
        builder.AddApplicationAuthentication();
        builder.AppApplicationAuthorization();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
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

    public static WebApplication MapApplicationMiddlewareAndEndpoints(this WebApplication app)
    {
        app.UseMiddleware<LogHeadersMiddleware>();
        
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "Returned {StatusCode} for path {RequestPath}";
        });

        app.UseSwagger();
        app.UseSwaggerUI();
    
        app.MapDisasters();

        return app;
    }
}