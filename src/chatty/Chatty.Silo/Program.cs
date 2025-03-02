using Chatty.Silo.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

builder.UseOrleans(siloBuilder =>
{
    siloBuilder.UseChattyOrleans(builder.Configuration, builder.Environment);
});

var app = builder.Build();

app.Run();