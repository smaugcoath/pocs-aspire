
var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithRedisInsight();

var postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin();

var postgresDb = postgres
    //.WithDataVolume(isReadOnly: false)
    .AddDatabase("postgresdb");

var apiService = builder.AddProject<Projects.Pocs_Aspire_ApiService>("apiservice")
    .WithReference(postgresDb);

builder.Build().Run();
