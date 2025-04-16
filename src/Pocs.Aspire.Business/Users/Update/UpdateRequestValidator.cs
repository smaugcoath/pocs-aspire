using FluentValidation;
using Pocs.Aspire.Business.Validations;
using Pocs.Aspire.Domain.Users;
using System;

namespace Pocs.Aspire.Business.Users.Update;
public class UpdateRequestValidator : AbstractValidator<UpdateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRequestValidator"/> class.
    /// </summary>
    public UpdateRequestValidator(IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));

        RuleFor(x => x.Id).ValidUserId();
        RuleFor(u => u.FirstName).ValidFirstName();
        RuleFor(u => u.LastName).ValidFirstName();
        RuleFor(u => u.Email).ValidEmail(userRepository);
    }
}
