var builder = DistributedApplication.CreateBuilder(args);

var sqllite = builder.AddSqlite("healthydb");

builder.AddProject<Projects.Healthy_Web>("Web")
    .WithReference(sqllite);

builder.Build().Run();