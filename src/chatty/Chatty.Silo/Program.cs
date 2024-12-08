// See https://aka.ms/new-console-template for more information

using System.Net;
using Chatty.Silo.Configuration;
using Chatty.Silo.Configuration.Serialization;
using Orleans.Configuration;
using Orleans.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans((_, siloBuilder) =>
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
    siloBuilder.Services.Configure<EndpointOptions>(options =>
    {
        var isDockerCompose = bool.Parse(Environment.GetEnvironmentVariable("IS_DOCKER_COMPOSE") ?? "false");
        if (isDockerCompose)
        {
            var dns = $"silo-{Environment.GetEnvironmentVariable("SILO_NUMBER")!}";
            options.AdvertisedIPAddress = Dns.GetHostEntry(dns).AddressList[0];
        }
        else
        {
            options.AdvertisedIPAddress = IPAddress.Loopback;
        }

        options.GatewayPort = int.Parse(Environment.GetEnvironmentVariable("GATEWAY_PORT")!);
        options.SiloPort = int.Parse(Environment.GetEnvironmentVariable("SILO_PORT")!);
    });
    siloBuilder.Services.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = ChattyOrleansConstants.Cluster.ClusterId;
        options.ServiceId = ChattyOrleansConstants.Cluster.ServiceId;
    });
    siloBuilder.Services.Configure<SiloOptions>(options =>
    {
        options.SiloName = $"silo-number-{Environment.GetEnvironmentVariable("SILO_NUMBER")}";
    });
    siloBuilder.Services.Configure<ClusterMembershipOptions>(options =>
    {
        options.DefunctSiloCleanupPeriod = TimeSpan.FromMinutes(1);
        options.DefunctSiloExpiration = TimeSpan.FromMinutes(1);
    });
    siloBuilder.Services.AddSerializer(sb => sb.AddApplicationSpecificSerialization());

    siloBuilder.Services.AddLogging();
}).UseConsoleLifetime();

var app = builder.Build();

app.Run();