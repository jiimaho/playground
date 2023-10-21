var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/disasters", context =>
{
    return Task.FromResult("Helo World!");
});

app.Run();