using Microsoft.EntityFrameworkCore;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Users;

namespace Pocs.Aspire.Infrastructure.Persistence;

public class AppDbContext : DbContext, IUnitOfWork
{
    internal DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
