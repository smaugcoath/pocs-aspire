﻿namespace Pocs.Aspire.Business.Users.GetById;

using FluentValidation;
using LanguageExt;
using Pocs.Aspire.Business.Validations;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class GetByIdService : IGetByIdService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<GetByIdRequest> _validator;

    public GetByIdService(IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<GetByIdRequest> validator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Either<Exception, GetByIdResponse>> GetByIdAsync(GetByIdRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return validationResult.ToValidationException();
        }

        var userId = request.ToDomain();

        var userOption = await _userRepository.GetByIdAsync(userId, cancellationToken);

        return userOption
            .ToEither<Exception>(new NotFoundException())
            .Bind<GetByIdResponse>(x => x.ToResponse());
    }


}
