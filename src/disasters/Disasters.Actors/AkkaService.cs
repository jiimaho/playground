using Akka.Actor;
using Akka.Actor.Setup;
using Akka.DependencyInjection;

namespace Disasters.Actors;

public class AkkaService : IHostedService
{
    private readonly ILogger<AkkaService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _appLifetime;
    private ActorSystemSetup _actorSystemSetup;
    private ActorSystem _actorSystem;

    public AkkaService(ILogger<AkkaService> logger, IServiceProvider serviceProvider, IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _appLifetime = appLifetime;
        Console.WriteLine("AkkaService created");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var bootstrap = BootstrapSetup.Create();
        
        // enable DI support inside this ActorSystem, if needed
        var diSetup = DependencyResolverSetup.Create(_serviceProvider);
        
        // merge this setup (and any others) together into ActorSystemSetup
        _actorSystemSetup = bootstrap.And(diSetup);
        
        _actorSystem = ActorSystem.Create("Disasters", _actorSystemSetup);
        
        var _ = _actorSystem.ActorOf(CarActor.Props, "RandomActor");
        
        _actorSystem.WhenTerminated.ContinueWith(tr =>
        {
            _appLifetime.StopApplication();
        }, cancellationToken);
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
    }
}