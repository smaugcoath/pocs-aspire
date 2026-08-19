namespace Pocs.Aspire.Tests.Business.Tests.Unit.Users.Update;

using LanguageExt;
using NSubstitute;
using Pocs.Aspire.Business.Users.Update;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Errors;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateServiceTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateService _sut;

    public UpdateServiceTests()
    {
        _sut = new UpdateService(_userRepository, _unitOfWork, new UpdateRequestValidator());
    }

    private static User ExistingUser(Guid id) =>
        User.From(UserId.From(id), FirstName.From("Old"), LastName.From("Name"), Email.From("old@example.com"));

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdateResponse_WhenRequestIsValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository
            .EmailExistsExceptForUser(Arg.Any<Email>(), Arg.Any<UserId?>(), Arg.Any<CancellationToken>())
            .Returns(false);
        _userRepository
            .GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .Returns(Prelude.Some(ExistingUser(id)));
        var request = new UpdateRequest(id, "New", "Name", "new@example.com");

        // Act
        var result = await _sut.UpdateAsync(request, TestContext.Current.CancellationToken);

        // Assert
        var response = result.Case.ShouldBeOfType<UpdateResponse>();
        response.FirstName.ShouldBe("New");
        await _userRepository.Received(1).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnValidationError_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new UpdateRequest(Guid.Empty, string.Empty, string.Empty, "not-an-email");

        // Act
        var result = await _sut.UpdateAsync(request, TestContext.Current.CancellationToken);

        // Assert
        var error = result.Case.ShouldBeOfType<ValidationError>();
        error.Errors.ShouldContain(e => e.Field == "LastName" && e.Message == "Last name is required.");
        await _userRepository.DidNotReceive().GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFoundError_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository
            .EmailExistsExceptForUser(Arg.Any<Email>(), Arg.Any<UserId?>(), Arg.Any<CancellationToken>())
            .Returns(false);
        _userRepository
            .GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .Returns(Option<User>.None);
        var request = new UpdateRequest(id, "New", "Name", "new@example.com");

        // Act
        var result = await _sut.UpdateAsync(request, TestContext.Current.CancellationToken);

        // Assert
        result.Case.ShouldBeOfType<NotFoundError>();
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnEmailAlreadyExistsError_WhenEmailIsAlreadyTakenByAnotherUser()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository
            .EmailExistsExceptForUser(Arg.Any<Email>(), Arg.Any<UserId?>(), Arg.Any<CancellationToken>())
            .Returns(true);
        var request = new UpdateRequest(id, "New", "Name", "new@example.com");

        // Act
        var result = await _sut.UpdateAsync(request, TestContext.Current.CancellationToken);

        // Assert
        result.Case.ShouldBeOfType<EmailAlreadyExistsError>();
        await _userRepository.DidNotReceive().GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>());
    }
}
