namespace Disasters.Api.Authorization;

public static class AuthorizationExtensions
{
    public static WebApplicationBuilder AppApplicationAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("MaPol", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("MyScheme");
            });
        });

        return builder;
    }    
}