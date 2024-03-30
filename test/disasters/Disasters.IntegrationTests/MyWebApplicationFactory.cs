using Disasters.Api.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using Xunit.Abstractions;

namespace Disasters.IntegrationTests;

[UsedImplicitly]
public class MyWebApplicationFactory : WebApplicationFactory<Program>
{
    private ILogger _testLogger = null!;
    private readonly List<LogEvent> _assertableLogs = [];
    public ITestOutputHelper? OutputHelper { get; set; }
    
    public Mock<IDisastersService> MockedDisastersService { get; } = new();
    public IReadOnlyList<LogEvent> AssertableLogs => _assertableLogs;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (OutputHelper is null)
        {
            throw new InvalidOperationException("You must set the OutputHelper property on the factory before running any test");
        }
        base.ConfigureWebHost(builder);
        InitializeTestLogger();
        
        builder.ConfigureAppConfiguration(ctx =>
        {
            ctx.AddInMemoryCollection(new Dictionary<string, string?>
            {
            });
        });
        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
        });
        builder.ConfigureServices(collection =>
        {
            RemoveIfExists<IConnectionMultiplexer>(collection);
            RemoveIfExists<IDisastersService>(collection);

            collection.AddSerilog(_testLogger);
            collection.Add(ServiceDescriptor.Singleton(MockedDisastersService.Object));
        });
    }

    private void InitializeTestLogger()
    {
        _testLogger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Sink(new DelegatingSink(e => _assertableLogs.Add(e))) // Let's us assert against logs
            .WriteTo.TestOutput(OutputHelper) // Writes logs to Xunit output console
            .CreateLogger();
    }

    private static void RemoveIfExists<T> (IServiceCollection collection)
    {
        var service = collection.FirstOrDefault(x => x.ServiceType == typeof(T));
        if (service is not null)
        {
            collection.Remove(service);
        }
    }
}