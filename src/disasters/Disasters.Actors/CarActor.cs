using Akka.Actor;
using Akka.Cluster.Tools.Singleton;

namespace Disasters.Actors;

public class CarActor : ReceiveActor
{
    private readonly IActorRef _counterProxy;

    public CarActor()
    {
        var cs = ClusterSingleton.Get(Context.System);
        // _counterProxy = cs.Init(SingletonActor.Create(GlobalCounterSingleton.Props, "GlobalCounter"));
        
        Become(Available);
        Console.WriteLine("CarActor created");
    }

    public void Available()
    {
        Receive<DriveTo>(a =>
        {
            _counterProxy.Tell(new object());
        });
    }
    
    public static Props Props => Akka.Actor.Props.Create(() => new CarActor());

    public record DriveTo;
}