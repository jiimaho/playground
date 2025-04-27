using Healthy.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

app.ConfigurePipeline();

app.Run();
