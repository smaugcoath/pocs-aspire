using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pocs.Aspire.Infrastructure.Persistence;

namespace Pocs.Aspire.AppHost;

internal class AppDbContextDesignTimeFactory :
IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var postgres = builder
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
