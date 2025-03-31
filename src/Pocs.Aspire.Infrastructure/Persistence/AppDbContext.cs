using Microsoft.EntityFrameworkCore;
using Pocs.Aspire.Domain;

namespace Pocs.Aspire.Infrastructure.Persistence;

public class AppDbContext: DbContext, IUnitOfWork
{
    internal DbSet<UserEntity> Users => Set<UserEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
