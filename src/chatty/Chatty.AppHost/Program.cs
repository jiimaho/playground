using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(
        c => c.WithImageTag("3.33.0")
            .WithContainerName("chatty-storage")
            .WithBlobPort(10000)
            .WithQueuePort(10001)
            .WithTablePort(10002));

var sensitiveTable = storage.AddTables("sensitive");
var clusterTable = storage.AddTables("clustering");
var stateBlob = storage.AddBlobs("grain-state");

var orleans = builder.AddOrleans("default")
    .WithClustering(clusterTable)
    .WithGrainStorage("chatty", stateBlob);

var silo = builder.AddProject<Chatty_Silo>(name: "silo")
    .WithReference(orleans)
    .WithReference(sensitiveTable);

var web = builder.AddProject<Chatty_Web>(name: "web")
    .WithReference(silo)
    .WaitFor(silo)
    .WithReference(orleans.AsClient())
    .WithExternalHttpEndpoints();

builder.Build().Run();