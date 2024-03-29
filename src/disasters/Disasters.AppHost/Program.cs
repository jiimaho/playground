var builder = DistributedApplication.CreateBuilder(args);

builder.AddContainer("aspire-grafana", "grafana/grafana");
builder.AddContainer("aspire-prometheus", "bitnami/prometheus");

var disastersEndpoint = builder.AddProject<Projects.Disasters_Api>("backend")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Production")
    .WithHttpEndpoint(name: "backend-two", hostPort: 8080)
    .GetEndpoint("backend-two");

builder.AddProject<Projects.Disasters_GraphQL>("graphql")
    .WithReference(disastersEndpoint);

builder.Build().Run();