using FluentValidation;
using Pocs.Aspire.Business.Validations;
using Pocs.Aspire.Domain.Users;
using System;

namespace Pocs.Aspire.Business.Users.Update;
public class UpdateRequestValidator : AbstractValidator<UpdateRequest>
{
    public UpdateRequestValidator(IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(userRepository);

        RuleFor(x => x.Id).ValidUserId();
        RuleFor(u => u.FirstName).ValidFirstName();
        RuleFor(u => u.LastName).ValidLastName();
        RuleFor(u => u.Email).ValidEmail();
    }
}
