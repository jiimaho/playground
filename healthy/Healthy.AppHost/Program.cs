var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Healthy_Web>("Web");

builder.Build().Run();