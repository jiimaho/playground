var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");


builder.Build().Run();