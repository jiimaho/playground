var builder = DistributedApplication.CreateBuilder(args);

var sqllite = builder.AddSqlite("healthydb");

var web = builder.AddProject<Projects.Healthy_Web>("web")
    .WithReference(sqllite);

builder.Build().Run();