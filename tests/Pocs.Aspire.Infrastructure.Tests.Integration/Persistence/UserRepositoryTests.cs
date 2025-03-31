namespace Pocs.Aspire.Infrastructure.Tests.Integration.Persistence;

using Microsoft.EntityFrameworkCore;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Infrastructure.Persistence;
using Shouldly;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

public class UserRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;
    private DbContextOptions<AppDbContext> DbContextOptions =>
    new DbContextOptionsBuilder<AppDbContext>()
        .UseNpgsql(_container.GetConnectionString())
        .Options;

    public UserRepositoryTests()
    {
        var postgreSqlBuilder = new PostgreSqlBuilder();
        _container = postgreSqlBuilder
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("password")
            .WithImage("postgres:15")
            .WithCleanUp(true)
            .Build();
    }

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync(TestContext.Current.CancellationToken);
        using var context = new AppDbContext(DbContextOptions);
        await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task CreateAsync_ShouldInsertUser()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test.user@example.com"
        };

        var expected = user;

        using var context = new AppDbContext(DbContextOptions);
        var repository = new UserRepository(context);

        // Act
        await repository.CreateAsync(user, TestContext.Current.CancellationToken);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var insertedUser = await context.Users
            .FirstAsync(x => x.UserId == user.Id.Value, TestContext.Current.CancellationToken);
        var actual = insertedUser.ToDomain();

        actual.ShouldBeEquivalentTo(expected);
    }
}
