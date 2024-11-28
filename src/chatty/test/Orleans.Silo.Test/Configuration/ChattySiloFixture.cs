using JetBrains.Annotations;
using Orleans.TestingHost;

namespace Orleans.Silo.Test.Configuration;

[UsedImplicitly]
public sealed class ChattySiloFixture : IDisposable
{
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