var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var web = builder.AddProject<Projects.Healthy_Web>("web")
    .WithReference(postgres);

builder.Build().Run();