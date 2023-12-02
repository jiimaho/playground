// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder();

builder.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering()
        .ConfigureLogging(loggingBuilder => loggingBuilder.AddConsole())
        .UseRedisReminderService(options =>
        {
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
            {
                EndPoints = { "localhost:6379" }
            };
        });
});

builder.UseConsoleLifetime();

using var host = builder.Build();
await host.RunAsync();