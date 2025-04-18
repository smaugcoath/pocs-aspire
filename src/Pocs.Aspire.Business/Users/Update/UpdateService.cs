﻿using FluentValidation;
using LanguageExt;
using Pocs.Aspire.Business.Validations;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Errors;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pocs.Aspire.Business.Users.Update;

internal class UpdateService : IUpdateService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateRequest> _validator;

    public UpdateService(IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<UpdateRequest> validator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Either<Error, UpdateResponse>> UpdateAsync(UpdateRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new ValidationError(validationResult.ToFieldErrors());
        }

        var email = Email.From(request.Email);
        var userId = UserId.From(request.Id);
        var isEmailExists = await _userRepository.EmailExistsExceptForUser(email, userId, cancellationToken);
        if (isEmailExists)
        {
            return new EmailAlreadyExistsError(email);
        }

        var updatedUser = request.ToDomain();

        var userOption = await _userRepository.GetByIdAsync(updatedUser.Id, cancellationToken);

        return await userOption
            .ToEither<Error>(new NotFoundError(nameof(User)))
            .ToAsync()
            .BindAsync<UpdateResponse>
            (
                async user =>
                {
                    (user.FirstName, user.LastName, user.Email) = (updatedUser.FirstName, updatedUser.LastName, updatedUser.Email);
                    await _userRepository.UpdateAsync(user, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return user.ToResponse();
                }
            ).ToEither();
    }
}
