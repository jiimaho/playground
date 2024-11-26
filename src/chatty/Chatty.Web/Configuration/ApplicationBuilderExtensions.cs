using Blazored.LocalStorage;
using Chatty.Silo.Configuration.Serialization;
using Chatty.Web.BackgroundService;
using Chatty.Web.Components;
using Chatty.Web.Endpoints;
using Chatty.Web.Hubs;
using Orleans.Configuration;
using Orleans.Serialization;

namespace Chatty.Web.Configuration;

public static class ApplicationBuilderExtensions
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        // Razor
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();
        builder.Services.AddBlazoredLocalStorage();
        
        // SignalR
        builder.Services.AddSignalR();
        
        // CORS
        builder.Services.AddCors();

        builder.Services.AddHostedService<UserOnlineNotifier>();
        
        // Orleans
        builder.Host.UseOrleansClient((_, clientBuilder) =>
        {
            clientBuilder.UseDynamoDBClustering(options => { options.Service = "eu-west-1"; });
            clientBuilder.Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "blazor-cluster";
                options.ServiceId = "blazor-service";
            });
            clientBuilder.Services.AddSerializer(sb => sb.AddApplicationSpecificSerialization());
        });
    }

    public static void ConfigureApplicationPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        app.MapHub<UserOnlineHub>("user-online");

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAntiforgery();
        app.UseEndpoints(erb => erb.AddReportRoute());

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode();
    }
}