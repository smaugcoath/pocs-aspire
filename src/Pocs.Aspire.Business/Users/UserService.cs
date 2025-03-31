namespace Pocs.Aspire.Business.Users;

using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using Pocs.Aspire.Business.Validations;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using System;
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

    public async Task<Result<User>> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(user);
        if (!validationResult.IsValid)
        {
            return new Result<User>(validationResult.ToValidationException());
        }
        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<Result<User>> GetAsync(UserId id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return new Result<User>(new NotFoundException());
        }

        return user;
    }

    public async Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(user);
        if (!validationResult.IsValid)
        {
            return new Result<User>(validationResult.ToValidationException());
        }
        var userExists = await _userRepository.UserExistsAsync(user.Id, cancellationToken);
        if (!userExists)
        {
            return new Result<User>(new NotFoundException());
        }
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}
