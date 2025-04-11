using FluentValidation;
using LanguageExt;
using Pocs.Aspire.Business.Validations;
using Pocs.Aspire.Domain;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pocs.Aspire.Business.Users.Create
{


    internal class CreateService : ICreateService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateRequest> _validator;

        public CreateService(IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<CreateRequest> validator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Either<Exception, CreateResponse>> CreateAsync(CreateRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationException();
            }

            User user = User.From(UserId.New(), FirstName.From(request.FirstName), LastName.From(request.LastName), Email.From(request.Email));

            await _userRepository.CreateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.ToResponse();
        }
    }
}
