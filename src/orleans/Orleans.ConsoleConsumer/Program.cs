// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.ConsoleConsumer;
using Orleans.Serialization;
using Orleans.Silo.Configuration.Serialization;

var host = new HostApplicationBuilder();

Environment.SetEnvironmentVariable("AWS_PROFILE", "localstack");

host.UseOrleansClient(clientBuilder =>
{
    clientBuilder.UseDynamoDBClustering(options =>
    {
        options.Service = "eu-west-1";
    });
    clientBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "blazor-cluster";
        options.ServiceId = "blazor-service";
    });
    clientBuilder.Services.AddSerializer(sb => sb.AddApplicationSpecificSerialization());
});

host.Services.AddHostedService<ChatBackgroundService>();

var app = host.Build();

await app.RunAsync();