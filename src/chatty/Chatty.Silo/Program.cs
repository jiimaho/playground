using Chatty.Silo.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans((context, siloBuilder) =>
{
    siloBuilder.UseChattyOrleans(context); 
}).UseConsoleLifetime();

var app = builder.Build();

app.Run();