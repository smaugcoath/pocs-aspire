using Microsoft.AspNetCore.Http;
using Pocs.Aspire.Business.Users.Create;
using Pocs.Aspire.Business.Users.GetById;
using Shouldly;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pocs.Aspire.ApiService.Tests.Functional.Users;

/// <summary>
/// Proves the users endpoints are actually wired to API versioning (URL-segment
/// versioned routes under api/v{version}/users), not just configured with no
/// endpoint applying it.
/// </summary>
public class ApiVersioningFunctionalTests : IClassFixture<AspireHostFixture>
{
    private readonly AspireHostFixture _fixture;

    public ApiVersioningFunctionalTests(AspireHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Get_GetById_ViaVersionedRoute_ReturnsOkWithUser_WhenUserExists()
    {
        // Arrange
        var client = _fixture.HttpClient;
        var cancellationToken = TestContext.Current.CancellationToken;
        var newUser = new CreateRequest("Ada", "Byron", "ada.byron.versioning@example.com");

        var createResponse = await client.PostAsJsonAsync("/api/v1/users", newUser, cancellationToken);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<CreateResponse>(cancellationToken);
        created.ShouldNotBeNull();

        var expected = new GetByIdResponse(created.Id, "Ada", "Byron", "ada.byron.versioning@example.com");

        // Act
        var response = await client.GetAsync(new Uri(client.BaseAddress!, $"/api/v1/users/{created.Id}"), cancellationToken);
        var actual = await response.Content.ReadFromJsonAsync<GetByIdResponse>(cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        actual.ShouldBeEquivalentTo(expected);
    }
}
