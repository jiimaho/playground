using Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

var builder = Host.CreateDefaultBuilder(args);

// Configure Orleans client
builder.UseOrleansClient(clientBuilder =>
{
    clientBuilder.UseLocalhostClustering();
});

// Configure logging
builder.ConfigureLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

using var host = builder.Build();

await host.StartAsync();

var client = host.Services.GetRequiredService<IClusterClient>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

// Generate a new wallboxId
var wallboxId = Guid.NewGuid();

// Create a new StartChargingDto object
var user = CreateStartChargingDto();

// Set the RequestContext
SetRequestContext();

logger.LogInformation("Issuing command to start charging for wallbox {WallboxId}", wallboxId);

var startChargingGrain = client.GetGrain<IStartChargingGrain>(wallboxId);
await startChargingGrain.StartCharging(user);

Console.ReadKey();

await host.StopAsync();

// Method to create a new StartChargingDto object
StartChargingDto CreateStartChargingDto()
{
    return new StartChargingDto { User = new UserDto { Name = "Jim" }, Current = 16 };
}

// Method to set the RequestContext
void SetRequestContext()
{
    RequestContext.Set("TraceId", "12345");
}