using NSubstitute;
using Pocs.Aspire.Business.User;
using Pocs.Aspire.Business.Users;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Users;
using Shouldly;

namespace Pocs.Aspire.Tests.Business.Tests.Unit.Users;

public class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserValidator _validator;
    private readonly IUserService _sut;

    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _validator = new();
        _sut = new UserService(_userRepository, _unitOfWork, _validator);
    }

    [Theory]
    [ClassData(typeof(InvalidUserTheoryData))]
    public async Task CreateAsync_ShouldReturnValidationError_WhenUserIsInvalid(
        User invalidUser,
        BusinessValidationException expectedException)
    {
        // Arrange

        // Act
        var actual = await _sut.CreateAsync(invalidUser, CancellationToken.None);

        // Assert
        actual.IsFaulted.ShouldBeTrue();
        actual.IfFail(actual =>
        {
            actual
                .ShouldBeOfType<BusinessValidationException>()
                .ShouldBeEquivalentTo(expectedException);
        });
    }

    [Theory]
    [ClassData(typeof(ValidUserTheoryData))]
    public async Task CreateAsync_ShouldReturnSuccess_WhenUserIsValid(User validUser)
    {
        // Arrange
        CancellationTokenSource cancellationTokenSource = new();
        var cancellationToken = cancellationTokenSource.Token;
        _userRepository
            .CreateAsync(validUser)
            .Returns(Task.CompletedTask);
        _unitOfWork
            .SaveChangesAsync(cancellationToken)
            .Returns(Task.FromResult(1));

        // Act
        var actual = await _sut.CreateAsync(validUser, cancellationToken);

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.IfSucc(user => user.ShouldBe(validUser));

        await _userRepository.Received(1).CreateAsync(validUser, cancellationToken);
        await _unitOfWork.Received(1).SaveChangesAsync(cancellationToken);
    }
}

public sealed class InvalidUserTheoryData : TheoryData<User, BusinessValidationException>
{
    public InvalidUserTheoryData()
    {
        Add(
            new() { FirstName = "", LastName = "Valid", Email = "valid@example.com" },
            new([new(nameof(User.FirstName), "First name is required.")])
        );

        Add(
            new() { FirstName = "Valid", LastName = "", Email = "valid@example.com" },
            new([new(nameof(User.LastName), "Last name is required.")])
        );

        Add(
            new() { FirstName = "Valid", LastName = "Valid", Email = "invalid-email" },
            new([new(nameof(User.Email), "A valid email is required.")])
        );

        Add(
            new() { FirstName = "", LastName = "", Email = "valid@example.com" },
            new
            ([
                new(nameof(User.FirstName), "First name is required."),
                new(nameof(User.LastName), "Last name is required.")
            ])
        );

        Add(
            new() { FirstName = "", LastName = "Valid", Email = "invalid-email" },
            new
            ([
                new(nameof(User.FirstName), "First name is required."),
                new(nameof(User.Email), "A valid email is required.")
            ])
        );

        Add(
            new() { FirstName = "Valid", LastName = "", Email = "invalid-email" },
            new
            ([
                new(nameof(User.LastName), "Last name is required."),
                new(nameof(User.Email), "A valid email is required.")
            ])
        );

        Add(
            new() { FirstName = "", LastName = "", Email = "invalid-email" },
            new
            ([
                new(nameof(User.FirstName), "First name is required."),
                new(nameof(User.LastName), "Last name is required."),
                new(nameof(User.Email), "A valid email is required.")
            ])
        );
    }
}

public sealed class ValidUserTheoryData : TheoryData<User>
{
    public ValidUserTheoryData()
    {
        Add(new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        });

        Add(new User
        {
            FirstName = new string('A', 50),
            LastName = new string('B', 50),
            Email = new string('c', 64) + "@" + new string('d', 35)
        });
    }
}