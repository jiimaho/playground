using Chatty.Silo.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

builder.AddKeyedAzureTableClient("sensitive");
builder.AddKeyedAzureTableClient("clustering");
builder.AddKeyedAzureBlobClient("grain-state");
builder.UseOrleans(siloBuilder =>
{
    siloBuilder.UseChattyOrleans(builder.Configuration, builder.Environment);
});

var app = builder.Build();

app.Run();