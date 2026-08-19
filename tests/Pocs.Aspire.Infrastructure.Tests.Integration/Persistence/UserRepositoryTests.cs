namespace Pocs.Aspire.Infrastructure.Tests.Integration.Persistence;

using Microsoft.EntityFrameworkCore;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using Pocs.Aspire.Infrastructure.Persistence;
using Shouldly;

using System;
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
            .WithImage("postgres:15")
            .WithCleanUp(true)
            .Build();
    }

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync(TestContext.Current.CancellationToken);
        using var context = new AppDbContext(DbContextOptions);
        await context.Database.MigrateAsync(TestContext.Current.CancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task CreateAsync_ShouldInsertUser()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var user = User.New(FirstName.From("Test"), LastName.From("User"), Email.From("test.user@example.com"));

        using var context = new AppDbContext(DbContextOptions);
        var repository = new UserRepository(context);

        // Act
        await repository.CreateAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // Assert
        using var verificationContext = new AppDbContext(DbContextOptions);
        var insertedUser = await verificationContext.Users
            .AsNoTracking()
            .FirstAsync(x => x.Id == user.Id, cancellationToken);

        insertedUser.FirstName.ShouldBe(user.FirstName);
        insertedUser.LastName.ShouldBe(user.LastName);
        insertedUser.Email.ShouldBe(user.Email);
    }

    // Regression test for the FindAsync(id, cancellationToken) bug: passing a bare key value and a
    // CancellationToken to the params object?[] overload made EF see two key values for a single-key
    // entity and throw at runtime. GetByIdAsync must build the key array explicitly.
    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var user = User.New(FirstName.From("Ada"), LastName.From("Lovelace"), Email.From("ada.lovelace@example.com"));

        using (var seedContext = new AppDbContext(DbContextOptions))
        {
            var seedRepository = new UserRepository(seedContext);
            await seedRepository.CreateAsync(user, cancellationToken);
            await seedContext.SaveChangesAsync(cancellationToken);
        }

        using var context = new AppDbContext(DbContextOptions);
        var repository = new UserRepository(context);

        // Act
        var result = await repository.GetByIdAsync(user.Id, cancellationToken);

        // Assert
        var found = result.Case.ShouldBeOfType<User>();
        found.Id.ShouldBe(user.Id);
        found.Email.ShouldBe(user.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNone_WhenUserDoesNotExist()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        using var context = new AppDbContext(DbContextOptions);
        var repository = new UserRepository(context);

        // Act
        var result = await repository.GetByIdAsync(UserId.New(), cancellationToken);

        // Assert
        result.IsNone.ShouldBeTrue();
    }
}
