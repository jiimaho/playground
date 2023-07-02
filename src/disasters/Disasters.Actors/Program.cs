using Disasters.Actors;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<AkkaService>();
    })
    .Build();

await host.RunAsync();