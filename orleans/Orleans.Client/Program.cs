using Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder(args);

builder.UseOrleansClient(clientBuilder =>
{
    clientBuilder.UseLocalhostClustering();
});

builder.ConfigureLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

using var host = builder.Build();

await host.StartAsync();

var client = host.Services.GetRequiredService<IClusterClient>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

var wallboxId = Guid.NewGuid();
var user = new StartChargingDto { User = new UserDto { Name = "Jim" }, Current = 16 };
logger.LogInformation("Issuing command to start charging for wallbox {WallboxId}", wallboxId);
var startChargingGrain = client.GetGrain<IStartChargingGrain>(wallboxId);
await startChargingGrain.StartCharging(user);

Console.ReadKey();

await host.StopAsync();