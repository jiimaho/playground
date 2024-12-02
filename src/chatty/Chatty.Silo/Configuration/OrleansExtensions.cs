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
            siloBuilder.UseCosmosClustering(
                configureOptions: options =>
                {
                    options.IsResourceCreationEnabled = false;
                    options.DatabaseName = "chatty";
                    options.ContainerName = "silo";
                    options.ConfigureCosmosClient(
                        context.Configuration.GetValue<string>("COSMOS_ENDPOINT")!,
                        new ManagedIdentityCredential(Environment.GetEnvironmentVariable("MANAGED_IDENTITY_CLIENT_ID")!));
                });

            siloBuilder.AddCosmosGrainStorage(
                name: "profileStore",
                configureOptions: options =>
                {
                    options.IsResourceCreationEnabled = false;
                    options.DatabaseName = "chatty";
                    options.ContainerName = "state";
                    options.ConfigureCosmosClient(
                        context.Configuration.GetValue<string>("COSMOS_ENDPOINT")!,
                        new ManagedIdentityCredential(Environment.GetEnvironmentVariable("MANAGED_IDENTITY_CLIENT_ID")!));
                });
        }

        // Silo ports & IP
        siloBuilder.Services.Configure<EndpointOptions>(options =>
        {
            var isDockerCompose = bool.Parse(Environment.GetEnvironmentVariable("IS_DOCKER_COMPOSE") ?? "false");
            if (isDockerCompose)
            {
                var dns = $"silo-{Environment.GetEnvironmentVariable("SILO_NUMBER")!}";
                options.AdvertisedIPAddress = Dns.GetHostEntry(dns).AddressList[0];
            }
            else if (context.HostingEnvironment.IsDevelopment())
            {
                options.AdvertisedIPAddress = IPAddress.Loopback;
            }

            options.GatewayPort = int.Parse(Environment.GetEnvironmentVariable("GATEWAY_PORT")!);
            options.SiloPort = int.Parse(Environment.GetEnvironmentVariable("SILO_PORT")!);
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
            options.SiloName = $"silo-number-{Environment.GetEnvironmentVariable("SILO_NUMBER")}";
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