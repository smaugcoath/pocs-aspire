namespace Pocs.Aspire.Tests.Business.Tests.Unit.Users.Create;

using LanguageExt;
using NSubstitute;
using Pocs.Aspire.Business.Users.Create;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Errors;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateServiceTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateService _sut;

    public CreateServiceTests()
    {
        _sut = new CreateService(_userRepository, _unitOfWork, new CreateUserRequestValidator());
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreateResponse_WhenRequestIsValid()
    {
        // Arrange
        _userRepository
            .EmailExistsExceptForUser(Arg.Any<Email>(), Arg.Any<UserId?>(), Arg.Any<CancellationToken>())
            .Returns(false);
        var request = new CreateRequest("Ada", "Lovelace", "ada@example.com");

        // Act
        var result = await _sut.CreateAsync(request, TestContext.Current.CancellationToken);

        // Assert
        var response = result.Case.ShouldBeOfType<CreateResponse>();
        response.Id.ShouldNotBe(Guid.Empty);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnValidationError_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new CreateRequest(string.Empty, string.Empty, "not-an-email");

        // Act
        var result = await _sut.CreateAsync(request, TestContext.Current.CancellationToken);

        // Assert
        var error = result.Case.ShouldBeOfType<ValidationError>();
        error.Errors.ShouldContain(e => e.Field == "FirstName");
        error.Errors.ShouldContain(e => e.Field == "LastName" && e.Message == "Last name is required.");
        error.Errors.ShouldContain(e => e.Field == "Email");
        await _userRepository.DidNotReceive().CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnEmailAlreadyExistsError_WhenEmailIsAlreadyTaken()
    {
        // Arrange
        _userRepository
            .EmailExistsExceptForUser(Arg.Any<Email>(), Arg.Any<UserId?>(), Arg.Any<CancellationToken>())
            .Returns(true);
        var request = new CreateRequest("Ada", "Lovelace", "ada@example.com");

        // Act
        var result = await _sut.CreateAsync(request, TestContext.Current.CancellationToken);

        // Assert
        result.Case.ShouldBeOfType<EmailAlreadyExistsError>();
        await _userRepository.DidNotReceive().CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
