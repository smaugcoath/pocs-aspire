namespace Pocs.Aspire.Infrastructure.Tests.Integration.Persistence;

using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using Pocs.Aspire.Infrastructure.Persistence;
using Shouldly;

using System;
using System.Threading;
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
        await context.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
    {
        // Arrange
        var expected = User.From(
            UserId.New(),
            FirstName.From("Ada"),
            LastName.From("Lovelace"),
            Email.From("ada.lovelace.getbyid@example.com"));

        await using (var seedContext = new AppDbContext(DbContextOptions))
        {
            seedContext.Add(expected);
            await seedContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        await using var context = new AppDbContext(DbContextOptions);
        var repository = new UserRepository(context);

        // Act
        var result = await repository.GetByIdAsync(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var actual = result.Match(
            Some: user => user,
            None: () => throw new InvalidOperationException("Expected the user to be found, but the repository returned None."));

        actual.ShouldBeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsOperationCanceled_WhenCancellationTokenIsAlreadyCanceled()
    {
        // Arrange
        await using var context = new AppDbContext(DbContextOptions);
        var repository = new UserRepository(context);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        var act = () => repository.GetByIdAsync(UserId.New(), cts.Token);

        // Assert
        await Should.ThrowAsync<OperationCanceledException>(act);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNone_WhenUserDoesNotExist()
    {
        // Arrange
        await using var context = new AppDbContext(DbContextOptions);
        var repository = new UserRepository(context);
        var missingId = UserId.New();
        var expected = Option<User>.None;

        // Act
        var result = await repository.GetByIdAsync(missingId, TestContext.Current.CancellationToken);

        // Assert
        result.ShouldBeEquivalentTo(expected);
    }
}
