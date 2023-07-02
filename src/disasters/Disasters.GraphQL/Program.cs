// See https://aka.ms/new-console-template for more information

using Akka.Actor;
using Akka.Hosting;


var builder = WebApplication.CreateBuilder(args);
    
builder.Services
    .AddAkka("disasters", configurationBuilder =>
    {
        configurationBuilder.WithActors((system, registry) =>
        {
            var graphqlActor = system.ActorOf(GraphQLActor.Props(), "GraphQLActor");
            registry.Register<GraphQLActor>(graphqlActor);
            // get singleton actor?
        });
    });

var app = builder.Build();

app.MapGet("/readme/{val}", (string val, IActorRegistry registry) =>
{
    var actor = registry.Get<GraphQLActor>();
    actor.Tell(val);
    return "This is just intended to be a playground for clustered Akka.NET and GraphQL.";
});

Console.WriteLine("GraphQL Server up and running!");
await app.RunAsync();

public class GraphQLActor : ReceiveActor
{
    public GraphQLActor()
    {
        ReceiveAny(o =>
        {
            Console.WriteLine($"Received \"{o.ToString()}\"");
            Console.WriteLine($"Received \"{o.ToString()}\"");
            Console.WriteLine("");
        });
    }
    
    public static Props Props() => Akka.Actor.Props.Create(() => new GraphQLActor());
}
