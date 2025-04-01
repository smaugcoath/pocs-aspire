using Microsoft.AspNetCore.Http;
using Pocs.Aspire.ApiService.Users;
using Pocs.Aspire.ApiService.Validations;
using Pocs.Aspire.Business.Users;
using Shouldly;
using System.Net.Http.Json;

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
        var newUser = new CreateUserRequest("Test", "User", "test.user@example.com");

        // Act
        var response = await client.PostAsJsonAsync("/api/users", newUser, cancellationToken);
        var actual = await response.Content.ReadFromJsonAsync<CreateUserResponse>(cancellationToken);

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

        var invalidUser = new CreateUserRequest("", "", "invalid-email");
        var expected = TypedResults.Problem(
               detail: "See the 'errors' property for details.",
               instance: "POST /api/users",
               statusCode: StatusCodes.Status400BadRequest,
               title: "Validation errors occurred.",
               type: nameof(BusinessValidationException),
               extensions: new Dictionary<string, object?>()
               {
                    { "FirstName", "First name is required."},
                    { "LastName", "Last name is required."},
                    { "Email", "A valid email is required."}
               }
           );


        // Act
        var response = await client.PostAsJsonAsync("/api/users", invalidUser, cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        // TODO: Assert problem details. Missing excluding to exclude traceId and other details
        // errorContent.ShouldBeEquivalentTo(expected);

    }
}




