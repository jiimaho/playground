// See https://aka.ms/new-console-template for more information

using System.Net;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans((_, siloBuilder) =>
{
    siloBuilder.UseDynamoDBClustering(options =>
    {
        options.CreateIfNotExists = true;
        options.Service = "eu-west-1";
    });
    siloBuilder.AddDynamoDBGrainStorage("blazorStore", options =>
    {
        options.CreateIfNotExists = true;
        options.Service = "eu-west-1";
    });

    siloBuilder.Services.Configure<EndpointOptions>(options => 
    { 
        options.AdvertisedIPAddress = IPAddress.Loopback;
        // options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, 8081);
        options.GatewayPort = int.Parse(Environment.GetEnvironmentVariable("GATEWAY_PORT"));
        // options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 8082);
        options.SiloPort = int.Parse(Environment.GetEnvironmentVariable("SILO_PORT"));
    });
    siloBuilder.Services.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "blazor-cluster";
        options.ServiceId = "blazor-service";
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
    
    siloBuilder.Services.AddLogging();
}).UseConsoleLifetime();

var app = builder.Build();

app.Run();