using Disasters.Api.Configuration.Authentication;
using Disasters.Api.Configuration.Authorization;
using Disasters.Api.Endpoints;
using Disasters.Api.Middleware;
using Disasters.Api.Services;
using Serilog;
using StackExchange.Redis;

namespace Disasters.Api.Configuration;

public static class ApplicationExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        builder.AddRedis("cache");

        builder.Services
            .AddOpenTelemetry()
            .WithTracing(tracing =>
                tracing.AddSource("Disasters.Api")
                    .AddSource("OpenTelemetry.Instrumentation.StackExchangeRedis"));

        builder.Host.UseSerilog((context, provider, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(provider)
                .Enrich.FromLogContext()
                .WriteTo.StandardConsole();
        });

        builder.Services.AddHttpClient(HttpClients.ReliefWeb, client =>
        {
            client.BaseAddress = new Uri("https://api.reliefweb.int");
            client.Timeout = TimeSpan.FromSeconds(5);
        });

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
            Log.Logger.Information("Using {DisasterService}", nameof(ReliefWebDisastersService));
            builder.Services.AddSingleton<IDisastersService, ReliefWebDisastersService>();
            builder.Services.Decorate<IDisastersService>((inner, sp) =>
                new CacheDisastersServiceDecorator(
                    inner,
                    sp.GetRequiredService<IConnectionMultiplexer>(),
                    sp.GetRequiredService<ILogger>(),
                    sp.GetRequiredService<TimeProvider>()));
            builder.Services.Decorate<IDisastersService>((inner, sp) =>
                new TraceDisasterServiceDecorator(
                    inner,
                    sp.GetRequiredService<ILogger>()));
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

        app.MapGetDisasters()
            .MapPostMarkSafe();

        return app;
    }
}