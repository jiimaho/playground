using Blazored.LocalStorage;
using Chatty.Silo.Configuration;
using Chatty.Silo.Configuration.Serialization;
using Chatty.Web.BackgroundService;
using Chatty.Web.Components;
using Chatty.Web.Endpoints;
using Chatty.Web.Hubs;
using MudBlazor.Services;
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

        // MudBlazor
        builder.Services.AddMudServices();

        // SignalR
        builder.Services.AddSignalR();

        // CORS
        builder.Services.AddCors();

        // builder.Services.AddHostedService<UserOnlineNotifier>();

        // Orleans
        builder.AddKeyedAzureTableClient("clustering");
        builder.UseOrleansClient(clientBuilder =>
        {
            clientBuilder.Services.AddSerializer(sb => sb.AddApplicationSpecificSerialization());
        });
        // builder.Host.UseOrleansClient((context, clientBuilder) =>
        // {
        //     // clientBuilder.UseDynamoDBClustering(options => 
        //     // {
        //     //     options.TableName = ChattyOrleansConstants.Cluster.ClusteringTableName;
        //     //     options.CreateIfNotExists = false;
        //     //     options.Service = ChattyOrleansConstants.Cluster.Region;
        //     // });
        //     if (!(context.HostingEnvironment.IsDevelopment() && context.Configuration.GetValue<bool>("IS_ASPIRE")))
        //     {
        //         clientBuilder.Configure<ClusterOptions>(options =>
        //         {
        //             options.ClusterId = ChattyOrleansConstants.Cluster.ClusterId;
        //             options.ServiceId = ChattyOrleansConstants.Cluster.ServiceId;
        //         });
        //     }
        //
        //     clientBuilder.Services.AddSerializer(sb => sb.AddApplicationSpecificSerialization());
        // });
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