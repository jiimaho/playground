using Microsoft.AspNetCore.Mvc;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Silo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseOrleansClient((context, clientBuilder) =>
{
    clientBuilder.UseDynamoDBClustering(options =>
    {
        options.Service = "eu-west-1";
    });
    clientBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "blazor-cluster";
        options.ServiceId = $"service-number-{Environment.GetEnvironmentVariable("SERIVCE_NUMBER")}";
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("rooms/{id}/messages", async (IClusterClient clusterClient, [FromRoute] string id, ChatMessageRequest request) =>
    {
        var chatRoom = clusterClient.GetGrain<IChatRoom>(id);
        var chatMessage = new ChatMessage("RestClient", request.Message);
        await chatRoom.PostMessage(chatMessage);
        return Results.Ok();
    })
    .WithName("PostMessage")
    .WithOpenApi();

app.Run();

record ChatMessageRequest([FromBody] string Message);