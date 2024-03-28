using Serilog;

namespace Disasters.GraphQL.Configuration;

public static class ApplicationExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        // Usual shit but also service discovery used by aspire and other things
        builder.AddServiceDefaults();

        builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource("Disasters.GraphQL"));

        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient("backend", client =>
        {
            client.BaseAddress = new Uri("http://_backend-two.backend");
        });

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog(Log.Logger);
        
        builder.Services.AddGraphQLServer()
            .AddQueryType<Query.Query>();

        return builder;
    }

    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplication MapApplicationEndpoints(this WebApplication app)
    {
        app.MapGraphQL()
            .AllowAnonymous()
            .WithName("graphql");
        
        app.MapBananaCakePop()
            .AllowAnonymous();

        return app;
    }
}