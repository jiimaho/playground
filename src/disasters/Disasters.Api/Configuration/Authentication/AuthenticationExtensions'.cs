namespace Disasters.Api.Configuration.Authentication;

public static class AuthenticationExtensions    
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplicationBuilder AddApplicationAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication()
            .AddScheme<AuthenticationHandlerOptions, AuthenticationHandler>("MyScheme", _ => { });

        return builder;
    }
}