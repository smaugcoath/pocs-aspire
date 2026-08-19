namespace Pocs.Aspire.Tests.Business.Tests.Unit.Users.GetById;

using LanguageExt;
using NSubstitute;
using Pocs.Aspire.Business.Users.GetById;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Errors;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

public class GetByIdServiceTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly GetByIdService _sut;

    public GetByIdServiceTests()
    {
        _sut = new GetByIdService(_userRepository, _unitOfWork, new GetByIdRequestValidator());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnResponse_WhenUserExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = User.From(UserId.From(id), FirstName.From("Ada"), LastName.From("Lovelace"), Email.From("ada@example.com"));
        _userRepository
            .GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .Returns(Prelude.Some(user));

        // Act
        var result = await _sut.GetByIdAsync(new GetByIdRequest(id), TestContext.Current.CancellationToken);

        // Assert
        var response = result.Case.ShouldBeOfType<GetByIdResponse>();
        response.Id.ShouldBe(id);
        response.FirstName.ShouldBe("Ada");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFoundError_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository
            .GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .Returns(Option<User>.None);

        // Act
        var result = await _sut.GetByIdAsync(new GetByIdRequest(id), TestContext.Current.CancellationToken);

        // Assert
        result.Case.ShouldBeOfType<NotFoundError>();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnValidationError_WhenIdIsEmpty()
    {
        // Act
        var result = await _sut.GetByIdAsync(new GetByIdRequest(Guid.Empty), TestContext.Current.CancellationToken);

        // Assert
        result.Case.ShouldBeOfType<ValidationError>();
        await _userRepository.DidNotReceive().GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>());
    }
}
