using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pocs.Aspire.ApiService.Users;
using Pocs.Aspire.Business;
using Pocs.Aspire.Infrastructure;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);

    };

});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddRedisOutputCache("cache");

builder.AddInfrastructureServices();

builder.Services.AddBusinessServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandler();
app.UseOutputCache();
// Enable Swagger middleware in development (or as needed).
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapDefaultEndpoints();
app.MapUserEndpoints();
app.ApplyDatabaseMigrations();

await app.RunAsync();
