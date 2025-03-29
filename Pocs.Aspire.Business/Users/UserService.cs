namespace Pocs.Aspire.Business.Users;

using FluentValidation;
using OneOf;
using OneOf.Types;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<User> _validator;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<User> validator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<User, ValidationError>> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(user);
        if (!validationResult.IsValid)
        {
            return new ValidationError(validationResult.Errors);
        }
        await _userRepository.CreateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<OneOf<User, NotFound>> GetAsync(UserId id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return new NotFound();
        }

        return user;
    }

    public async Task<OneOf<User, NotFound, ValidationError>> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(user);
        if (!validationResult.IsValid)
        {
            return new ValidationError(validationResult.Errors);
        }
        var userExists = await _userRepository.UserExistsAsync(user.Id);
        if (!userExists)
        {
            return new NotFound();
        }
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}
