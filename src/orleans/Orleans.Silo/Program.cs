// See https://aka.ms/new-console-template for more information

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans((ctx, siloBuilder) =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.Services.AddLogging();
});

var app = builder.Build();

app.Run();
