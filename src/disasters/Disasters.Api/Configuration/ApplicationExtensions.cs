using Disasters.Api.Authentication;
using Disasters.Api.Authorization;
using Disasters.Api.Disasters;

namespace Disasters.Api.Configuration;

public static class ApplicationExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
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