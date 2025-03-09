using Chatty.MinimalApi.Endpoints;
using Chatty.Silo.Configuration.Serialization;
using FluentValidation;
using Orleans.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<ChatMessageRequest>, ChatMessageValidator>();
builder.Services.AddScoped<IValidator<GetMessagesRequest>, GetMessagesRequestValidator>();

builder.AddServiceDefaults();

builder.AddKeyedAzureTableClient("clustering");

builder.Host.UseOrleansClient((context, clientBuilder) =>
{
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

app.MapPostMessageEndpoint();
app.MapGetMessagesEndpoint();
app.MapPostRandomMessageEndpoint();

await app.RunAsync();