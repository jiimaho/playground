using Bogus;
using Chatty.Silo.Primitives;
using JetBrains.Annotations;
using NodaTime.Extensions;
using Orleans.TestingHost;

namespace Orleans.Silo.Test.Configuration;

[UsedImplicitly]
public sealed class ChattySiloFixture : IDisposable
{
    private static DateTimeOffset SomeTime = DateTimeOffset.Now;
    
    public readonly Faker<ChatMessage> ChatMessageFaker = new Faker<ChatMessage>()
        .RuleFor(f => f.Message, f => f.Lorem.Sentence(10))
        .RuleFor(f => f.Username, f => new Username(f.Person.UserName))
        .RuleFor(f => f.Timestamp, SomeTime)
        .RuleFor(f => f.TimeStampNoda, _ => SomeTime.ToZonedDateTime())
        .RuleFor(f => f.ChatRoomId, _ => "all");
    
    public TestCluster Cluster { get; } 

    public ChattySiloFixture()
    {
        var builder = new TestClusterBuilder();
        builder.AddSiloBuilderConfigurator<ChattySiloConfigurator>();
        builder.AddClientBuilderConfigurator<ChattySiloClientConfigurator>();
        Cluster = builder.Build();
        Cluster.Deploy();
    }

    void IDisposable.Dispose() => Cluster.StopAllSilos();
}

[CollectionDefinition(nameof(OrleansCollection))]
public class OrleansCollection : ICollectionFixture<ChattySiloFixture>;