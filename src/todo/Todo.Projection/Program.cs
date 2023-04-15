// See https://aka.ms/new-console-template for more information

using EventStore.Client;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Starting projection...");

var settings = EventStoreClientSettings.Create("esdb://localhost:2113?tls=false");
var client = new EventStoreClient(settings);

var cts = new CancellationTokenSource();
var token = cts.Token;

await client.SubscribeToStreamAsync(
    "todos", 
    FromStream.End, 
    (subscription, @event, arg3) =>
    {
        Console.WriteLine($"Just got event {System.Text.Json.JsonSerializer.Deserialize<TodoEvent>(@event.Event.Data.ToArray()).Name}");
        return Task.CompletedTask;
    },
    subscriptionDropped: (subscription, reason, arg3) =>
    {
        Console.WriteLine("Subscription dropped");
    },
    cancellationToken: token);
Console.WriteLine("Projection started.");

var builder = new HostBuilder().UseConsoleLifetime();
var app = builder.Build();

app.WaitForShutdown();
cts.Cancel();
await Task.Delay(3000);

Console.WriteLine("Shutdown...");