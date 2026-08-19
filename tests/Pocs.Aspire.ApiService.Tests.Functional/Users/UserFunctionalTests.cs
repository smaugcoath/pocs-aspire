using Microsoft.AspNetCore.Http;
using Pocs.Aspire.Business.Users.Create;
using Pocs.Aspire.Business.Users.GetById;
using Pocs.Aspire.Business.Users.Update;
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

    private static CreateRequest NewCreateRequest(string? email = null) =>
        new("Test", "User", email ?? $"{Guid.NewGuid():N}@example.com");

    [Fact]
    public async Task Post_CreateUser_ReturnsCreatedWithCorrectLocation_WhenInputIsValid()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var newUser = NewCreateRequest();

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
    public async Task Post_CreateUser_ReturnsBadRequest_WhenInputIsInvalid()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var invalidUser = new CreateRequest("", "", "invalid-email");

        // Act
        var response = await client.PostAsJsonAsync("/api/users", invalidUser, cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>(cancellationToken);
        problem.ShouldNotBeNull();
        problem.Errors.ShouldContainKey("FirstName");
        problem.Errors.ShouldContainKey("LastName");
        problem.Errors.ShouldContainKey("Email");
        problem.Errors["LastName"].ShouldContain("Last name is required.");
    }

    [Fact]
    public async Task Post_CreateUser_ReturnsConflict_WhenEmailAlreadyExists()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var email = $"{Guid.NewGuid():N}@example.com";
        await client.PostAsJsonAsync("/api/users", NewCreateRequest(email), cancellationToken);

        // Act
        var response = await client.PostAsJsonAsync("/api/users", NewCreateRequest(email), cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Get_GetById_ReturnsOkWithUser_WhenUserExists()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var createResponse = await client.PostAsJsonAsync("/api/users", NewCreateRequest(), cancellationToken);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateResponse>(cancellationToken);

        // Act
        var response = await client.GetAsync(new Uri($"/api/users/{created!.Id}", UriKind.Relative), cancellationToken);
        var actual = await response.Content.ReadFromJsonAsync<GetByIdResponse>(cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        actual.ShouldNotBeNull();
        actual.Id.ShouldBe(created.Id);
    }

    [Fact]
    public async Task Get_GetById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var response = await client.GetAsync(new Uri($"/api/users/{Guid.NewGuid()}", UriKind.Relative), cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_UpdateUser_ReturnsOkWithUpdatedUser_WhenInputIsValid()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var createResponse = await client.PostAsJsonAsync("/api/users", NewCreateRequest(), cancellationToken);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateResponse>(cancellationToken);
        var updateRequest = new UpdateRequest(created!.Id, "Updated", "Name", $"{Guid.NewGuid():N}@example.com");

        // Act
        var response = await client.PutAsJsonAsync($"/api/users/{created.Id}", updateRequest, cancellationToken);
        var actual = await response.Content.ReadFromJsonAsync<UpdateResponse>(cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        actual.ShouldNotBeNull();
        actual.FirstName.ShouldBe("Updated");
        actual.LastName.ShouldBe("Name");
    }

    [Fact]
    public async Task Put_UpdateUser_ReturnsConflict_WhenEmailAlreadyExistsForAnotherUser()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var existingEmail = $"{Guid.NewGuid():N}@example.com";
        await client.PostAsJsonAsync("/api/users", NewCreateRequest(existingEmail), cancellationToken);

        var otherCreateResponse = await client.PostAsJsonAsync("/api/users", NewCreateRequest(), cancellationToken);
        var other = await otherCreateResponse.Content.ReadFromJsonAsync<CreateResponse>(cancellationToken);
        var updateRequest = new UpdateRequest(other!.Id, "Updated", "Name", existingEmail);

        // Act
        var response = await client.PutAsJsonAsync($"/api/users/{other.Id}", updateRequest, cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }
}
