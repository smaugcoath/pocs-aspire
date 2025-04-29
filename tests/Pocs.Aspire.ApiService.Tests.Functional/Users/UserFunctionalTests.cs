using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Pocs.Aspire.Business.Users.Create;

using Shouldly;

namespace Pocs.Aspire.ApiService.Tests.Functional.Users;

public class UserFunctionalTests : IClassFixture<AspireHostFixture>
{
    private readonly AspireHostFixture _fixture;
    private readonly ITestOutputHelper _output;

    public UserFunctionalTests(AspireHostFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task Post_CreateUser_ReturnsCreatedWithCorrectLocation_WhenInputIsValid()
    {
        // Arrange
        _output.WriteLine("Test starting.");
        //var client = _fixture.HttpClient;
        _output.WriteLine($"Client config: {_fixture.HttpClient.BaseAddress}");
        var cancellationToken = TestContext.Current.CancellationToken;
        var newUser = new CreateRequest("Test", "User", "test.user@example.com");

        // Act
        var response = await _fixture.HttpClient.PostAsJsonAsync("/api/users", newUser, cancellationToken);
        var actual = await response.Content.ReadFromJsonAsync<CreateResponse>(cancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        actual.ShouldNotBeNull();
        actual.Id.ShouldNotBe(Guid.Empty);
        var expectedUri = new Uri(_fixture.HttpClient.BaseAddress!, $"/api/users/{actual.Id}");
        response.Headers.Location.ShouldBe(expectedUri);
    }

}




