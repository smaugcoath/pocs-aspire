namespace Pocs.Aspire.Business.Tests.Unit.Users.GetById;

using Pocs.Aspire.Business.Users.GetById;
using Shouldly;
using System;
using System.Threading.Tasks;

public class GetByIdRequestValidatorTests
{
    [Fact]
    public async Task Validate_ReturnsInvalid_WhenIdIsEmpty()
    {
        // Arrange
        var validator = new GetByIdRequestValidator();
        var request = new GetByIdRequest(Guid.Empty);

        // Act
        var result = await validator.ValidateAsync(request, TestContext.Current.CancellationToken);

        // Assert
        result.IsValid.ShouldBeFalse();
    }

    [Fact]
    public async Task Validate_ReturnsValid_WhenIdIsNotEmpty()
    {
        // Arrange
        var validator = new GetByIdRequestValidator();
        var request = new GetByIdRequest(Guid.Parse("00000000-0000-0000-0000-0000000000ee"));

        // Act
        var result = await validator.ValidateAsync(request, TestContext.Current.CancellationToken);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
