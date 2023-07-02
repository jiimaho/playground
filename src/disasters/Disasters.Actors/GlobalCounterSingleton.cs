using Akka.Actor;

namespace Disasters.Actors;

public class GlobalCounterSingleton : UntypedActor
{
    private int Counter { get; set; }

    protected override void OnReceive(object message)
    {
        Counter++;
    }

    public static Props Props => Akka.Actor.Props.Create(() => new GlobalCounterSingleton());
}