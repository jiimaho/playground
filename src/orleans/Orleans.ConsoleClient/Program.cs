// See https://aka.ms/new-console-template for more information

using ConsoleApp1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostApplicationBuilder();

host.Services.AddOrleansClient(builder =>
{
    builder.UseLocalhostClustering();

});

host.Services.AddHostedService<ChatBackgroundService>();

var app = host.Build();

await app.RunAsync();