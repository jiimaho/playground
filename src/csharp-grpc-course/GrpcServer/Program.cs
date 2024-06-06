using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Server;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Listen(IPAddress.Loopback, 5252, listenOptions =>  listenOptions.Protocols = HttpProtocols.Http2);
    });
    builder.Services.AddGrpc();

    var app = builder.Build();

    app.MapGrpcService<CalculatorServiceImpl>();
    
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}