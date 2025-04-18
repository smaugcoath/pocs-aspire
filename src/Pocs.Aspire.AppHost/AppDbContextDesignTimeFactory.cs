using Aspire.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pocs.Aspire.Infrastructure.Persistence;

namespace Pocs.Aspire.AppHost;

internal class AppDbContextDesignTimeFactory :
IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        builder
            .AddPostgres("postgres")
            .AddDatabase("postgresdb");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("postgresdb");
        return new AppDbContext(optionsBuilder.Options);
    }
    //public sealed class DataContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
    //{
    //    public AppDbContext CreateDbContext(string[] args)
    //        => new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseNpgsql().Options);
    //}
}
