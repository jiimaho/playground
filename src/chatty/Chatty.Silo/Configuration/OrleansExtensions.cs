using System.Net;
using Chatty.Silo.Configuration.Serialization;
using JetBrains.Annotations;
using Orleans.Configuration;
using Orleans.Serialization;

namespace Chatty.Silo.Configuration;

public static class OrleansExtensions
{
    [UsedImplicitly]
    // public static ISiloBuilder UseChattyOrleans(this ISiloBuilder siloBuilder, HostBuilderContext context)
    public static ISiloBuilder UseChattyOrleans(this ISiloBuilder siloBuilder, IConfiguration configuration, IWebHostEnvironment environment)
    {
        // DynamoDB
        // if (context.HostingEnvironment.IsDevelopment())
        // {
        //     siloBuilder.UseDynamoDBClustering(options =>
        //     {
        //         options.TableName = ChattyOrleansConstants.Cluster.ClusteringTableName;
        //         options.Service = ChattyOrleansConstants.Cluster.Region;
        //         options.CreateIfNotExists = true;
        //     });
        //     siloBuilder.AddDynamoDBGrainStorage(ChattyOrleansConstants.Storage.Name, options =>
        //     {
        //         options.TableName = ChattyOrleansConstants.Storage.GrainStateTableName;
        //         options.Service = ChattyOrleansConstants.Cluster.Region;
        //         options.CreateIfNotExists = true;
        //     });
        // }

        // Cosmos
        // if (!(context.HostingEnvironment.IsDevelopment() && context.Configuration.GetValue<bool>("IS_ASPIRE")))
        // {
        //     var managedIdentityClientId = context.Configuration.GetValue<string>("MANAGED_IDENTITY_CLIENT_ID")!;
        //     var cosmosEndpoint = context.Configuration.GetValue<string>("COSMOS_ENDPOINT")!;
        //     siloBuilder.UseCosmosClustering(
        //         configureOptions: options =>
        //         {
        //             options.IsResourceCreationEnabled = false;
        //             options.DatabaseName = "chatty";
        //             options.ContainerName = "silo";
        //             options.ConfigureCosmosClient(cosmosEndpoint,
        //                 new ManagedIdentityCredential(managedIdentityClientId));
        //         });
        //
        //     siloBuilder.AddCosmosGrainStorage(
        //         name: "profileStore",
        //         configureOptions: options =>
        //         {
        //             options.IsResourceCreationEnabled = false;
        //             options.DatabaseName = "chatty";
        //             options.ContainerName = "state";
        //             options.ConfigureCosmosClient(cosmosEndpoint,
        //                 new ManagedIdentityCredential(managedIdentityClientId));
        //         });
        // }

        var isDockerCompose = configuration.GetValue<bool>("IS_DOCKER_COMPOSE");
        var siloNumber = configuration.GetValue<string>("SILO_NUMBER")!;

        // Silo ports & IP
        siloBuilder.Services.Configure<EndpointOptions>(options =>
        {
            // Docker
            if (isDockerCompose)
            {
                var dns = $"silo-{siloNumber}";
                options.AdvertisedIPAddress = Dns.GetHostEntry(dns).AddressList[0];
                options.GatewayPort = configuration.GetValue<int>("GATEWAY_PORT");
                options.SiloPort = configuration.GetValue<int>("SILO_PORT");
            }
            // Aspire
            else if (environment.IsDevelopment() && configuration.GetValue<bool>("IS_ASPIRE"))
            {
                // noop
            }
            // Cloud
            else
            {
                options.AdvertisedIPAddress = IPAddress.Loopback;
                options.GatewayPort = configuration.GetValue<int>("GATEWAY_PORT");
                options.SiloPort = configuration.GetValue<int>("SILO_PORT");
            }
        });

        // Not aspire
        if (!(environment.IsDevelopment() && configuration.GetValue<bool>("IS_ASPIRE")))
        {
            // Cluster id
            siloBuilder.Services.Configure<ClusterOptions>(options =>
            {
                options.ClusterId = ChattyOrleansConstants.Cluster.ClusterId;
                options.ServiceId = ChattyOrleansConstants.Cluster.ServiceId;
            });
            
            // Silo name
            siloBuilder.Services.Configure<SiloOptions>(options => { options.SiloName = $"silo-number-{siloNumber}"; });
        }

        // Silo cleanup
        siloBuilder.Services.Configure<ClusterMembershipOptions>(options =>
        {
            options.DefunctSiloCleanupPeriod = TimeSpan.FromMinutes(1);
            options.DefunctSiloExpiration = TimeSpan.FromMinutes(1);
        });

        siloBuilder.Services.AddSerializer(sb => sb.AddApplicationSpecificSerialization());

        siloBuilder.Services.AddLogging();

        return siloBuilder;
    }
}