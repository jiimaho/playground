using System.Net;
using GrpcServer;
using Microsoft.AspNetCore.Server.Kestrel.Core;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Listen(IPAddress.Loopback, 5252, listenOptions =>  listenOptions.Protocols = HttpProtocols.Http2);
    });
    builder.Services.AddGrpc();
    builder.Services.AddGrpcReflection();

    var app = builder.Build();

    app.MapGrpcService<CalculatorServiceImpl>();
    app.MapGrpcReflectionService();
    
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}