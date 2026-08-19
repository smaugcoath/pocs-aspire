using Microsoft.AspNetCore.Http;
using Pocs.Aspire.Business.Users.Create;
using Pocs.Aspire.Business.Users.GetById;
using Shouldly;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pocs.Aspire.ApiService.Tests.Functional.Users;

public class UserFunctionalTests : IClassFixture<AspireHostFixture>
{
    private readonly AspireHostFixture _fixture;

    public UserFunctionalTests(AspireHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Post_CreateUser_ReturnsCreatedWithCorrectLocation_WhenInputIsValid()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var newUser = new CreateRequest("Test", "User", "test.user@example.com");

        // Act
        var response = await client.PostAsJsonAsync("/api/users", newUser, cancellationToken);
        var actual = await response.Content.ReadFromJsonAsync<CreateResponse>(cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        actual.ShouldNotBeNull();
        actual.Id.ShouldNotBe(Guid.Empty);
        var expectedUri = new Uri(client.BaseAddress!, $"/api/users/{actual.Id}");
        response.Headers.Location.ShouldBe(expectedUri);
    }

    [Fact]
    public async Task Get_GetById_ReturnsOkWithUser_WhenUserExists()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var newUser = new CreateRequest("Grace", "Hopper", "grace.hopper.getbyid@example.com");

        var createResponse = await client.PostAsJsonAsync("/api/users", newUser, cancellationToken);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateResponse>(cancellationToken);
        created.ShouldNotBeNull();

        var expected = new GetByIdResponse(created.Id, "Grace", "Hopper", "grace.hopper.getbyid@example.com");

        // Act
        var response = await client.GetAsync(new Uri(client.BaseAddress!, $"/api/users/{created.Id}"), cancellationToken);
        var actual = await response.Content.ReadFromJsonAsync<GetByIdResponse>(cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        actual.ShouldBeEquivalentTo(expected);
    }

    [Fact]
    public async Task Get_GetById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var missingId = Guid.Parse("00000000-0000-0000-0000-0000000000aa");

        // Act
        var response = await client.GetAsync(new Uri(client.BaseAddress!, $"/api/users/{missingId}"), cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
