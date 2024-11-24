using Chatty.Web;
using Chatty.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

app.ConfigureApplicationPipeline();

app.Run();