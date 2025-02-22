using Chatty.Silo.Configuration.Serialization;
using Chatty.Silo.Features.Chatroom.Grains;
using Chatty.Silo.Primitives;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Orleans.ChatClient;
using Orleans.Configuration;
using Orleans.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<ChatMessageRequest>, ChatMessageValidator>();

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
    clientBuilder.Services.AddSerializer(sb => sb.AddApplicationSpecificSerialization());

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost(
    "rooms/{id}/messages",
    async (IValidator<ChatMessageRequest> validator,
        IClusterClient clusterClient,
        [FromRoute] string id,
        ChatMessageRequest request) =>
    {
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.IsValid == false)
        {
            return Results.BadRequest(validationResult.Errors);
        }
        var chatRoom = clusterClient.GetGrain<IChatRoom>(id);
        var chatMessage = ChatMessage.Create(Username.Create("RestClient"), request.Message, id);
        await chatRoom.PostMessage(chatMessage);
        return Results.Ok();
    })
    .WithName("PostMessage")
    .WithOpenApi();

await app.RunAsync();