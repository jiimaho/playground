namespace Disasters.Api.Authentication;

public static class AuthenticationExtensions    
{
    public static WebApplicationBuilder AddApplicationAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication()
            .AddScheme<AuthenticationHandlerOptions, AuthenticationHandler>("MyScheme", options => { });

        return builder;
    }
}