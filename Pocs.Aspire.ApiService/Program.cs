using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pocs.Aspire.ApiService.Extensions;
using Pocs.Aspire.Business;
using Pocs.Aspire.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.WithCustomProperties(context.HttpContext);
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add API versioning.
builder.Services.AddApiVersioning(options =>
{
    // Set the default API version.
    options.DefaultApiVersion = new ApiVersion(1, 0);
    // Assume the default version when not specified.
    options.AssumeDefaultVersionWhenUnspecified = true;
    // Report API versions in responses.
    options.ReportApiVersions = true;
});
// Add a versioned API explorer for Swagger integration.
builder.Services.AddApiVersioning().AddApiExplorer(options =>
{
    // Format the version in the group name (e.g. "v1").
    options.GroupNameFormat = "'v'VVV";
    // Substitute the version in the URL.
    options.SubstituteApiVersionInUrl = true;
});
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
app.MapControllers();
app.AddInfrastructure();

app.Run();