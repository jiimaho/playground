using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator(c => c.WithImageTag("3.33.0"));
var clusterTable = storage.AddTables("clustering");
var stateBlob = storage.AddBlobs("grain-state");

var orleans = builder.AddOrleans("default")
    .WithClustering(clusterTable)
    .WithGrainStorage("chatty", stateBlob);

var silo = builder.AddProject<Chatty_Silo>(name: "silo") 
    .WithEnvironment("IS_ASPIRE", "true")
    .WithReference(orleans);

var web = builder.AddProject<Chatty_Web>(name: "web") 
    .WithReference(silo).WaitFor(silo)
    .WithEnvironment("IS_ASPIRE", "true")
    .WithReference(orleans.AsClient())
    .WithExternalHttpEndpoints();

builder.Build().Run();