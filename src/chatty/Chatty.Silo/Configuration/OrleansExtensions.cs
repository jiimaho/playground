using System.Net;
using Azure;
using Azure.Identity;
using Chatty.Silo.Configuration.Serialization;
using JetBrains.Annotations;
using Microsoft.Azure.Cosmos;
using Orleans.Configuration;
using Orleans.Serialization;

namespace Chatty.Silo.Configuration;

public static class OrleansExtensions
{
    [UsedImplicitly]
    public static ISiloBuilder UseChattyOrleans(this ISiloBuilder siloBuilder, HostBuilderContext context)
    {
        // Local
        if (context.HostingEnvironment.IsDevelopment())
        {
            siloBuilder.UseDynamoDBClustering(options =>
            {
                options.TableName = ChattyOrleansConstants.Cluster.ClusteringTableName;
                options.Service = ChattyOrleansConstants.Cluster.Region;
                options.CreateIfNotExists = true;
            });
            siloBuilder.AddDynamoDBGrainStorage(ChattyOrleansConstants.Storage.Name, options =>
            {
                options.TableName = ChattyOrleansConstants.Storage.GrainStateTableName;
                options.Service = ChattyOrleansConstants.Cluster.Region;
                options.CreateIfNotExists = true;
            });
        }

        // Azure
        if (!context.HostingEnvironment.IsDevelopment())
        {
            var managedIdentityClientId = context.Configuration.GetValue<string>("MANAGED_IDENTITY_CLIENT_ID")!;
            var cosmosEndpoint = context.Configuration.GetValue<string>("COSMOS_ENDPOINT")!;
            siloBuilder.UseCosmosClustering(
                configureOptions: options =>
                {
                    options.IsResourceCreationEnabled = false;
                    options.DatabaseName = "chatty";
                    options.ContainerName = "silo";
                    options.ConfigureCosmosClient(cosmosEndpoint,
                        new ManagedIdentityCredential(managedIdentityClientId));
                });

            siloBuilder.AddCosmosGrainStorage(
                name: "profileStore",
                configureOptions: options =>
                {
                    options.IsResourceCreationEnabled = false;
                    options.DatabaseName = "chatty";
                    options.ContainerName = "state";
                    options.ConfigureCosmosClient(cosmosEndpoint,
                        new ManagedIdentityCredential(managedIdentityClientId));
                });
        }

        var isDockerCompose = context.Configuration.GetValue<bool>("IS_DOCKER_COMPOSE");
        var siloNumber = context.Configuration.GetValue<string>("SILO_NUMBER")!;

        // Silo ports & IP
        siloBuilder.Services.Configure<EndpointOptions>(options =>
        {
            if (isDockerCompose)
            {
                var dns = $"silo-{siloNumber}";
                options.AdvertisedIPAddress = Dns.GetHostEntry(dns).AddressList[0];
            }
            else if (context.HostingEnvironment.IsDevelopment())
            {
                options.AdvertisedIPAddress = IPAddress.Loopback;
            }

            options.GatewayPort = context.Configuration.GetValue<int>("GATEWAY_PORT");
            options.SiloPort = context.Configuration.GetValue<int>("SILO_PORT");
        });

        // Cluster id
        siloBuilder.Services.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = ChattyOrleansConstants.Cluster.ClusterId;
            options.ServiceId = ChattyOrleansConstants.Cluster.ServiceId;
        });

        // Silo name
        siloBuilder.Services.Configure<SiloOptions>(options =>
        {
            options.SiloName = $"silo-number-{siloNumber}";
        });

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