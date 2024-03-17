var builder = DistributedApplication.CreateBuilder(args);

builder.AddContainer("aspire-grafana", "grafana/grafana");
builder.AddContainer("aspire-prometheus", "bitnami/prometheus");

var disastersEndpoint = builder.AddProject<Projects.Disasters_Api>("backend")
    .WithHttpEndpoint(name: "disasters")
    .GetEndpoint("disasters");

builder.AddProject<Projects.Disasters_GraphQL>("graphql")
    .WithReference(disastersEndpoint);

builder.Build().Run();