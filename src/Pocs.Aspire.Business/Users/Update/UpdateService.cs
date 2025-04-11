using FluentValidation;
using LanguageExt;
using Pocs.Aspire.Business.Validations;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Users;
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

    public async Task<EitherAsync<Exception, UpdateResponse>> UpdateAsync(UpdateRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return validationResult.ToValidationException();
        }

        var updatedUser = request.ToDomain();

        var userOption = await _userRepository.GetByIdAsync(updatedUser.Id, cancellationToken);

        return userOption
            .ToEither<Exception>(new NotFoundException())
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
            );
    }
}
