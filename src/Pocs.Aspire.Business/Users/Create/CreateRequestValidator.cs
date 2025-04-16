using FluentValidation;
using Pocs.Aspire.Business.Validations;
using Pocs.Aspire.Domain.Users;
using System;

namespace Pocs.Aspire.Business.Users.Create;
public class CreateUserRequestValidator : AbstractValidator<CreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserRequestValidator"/> class.
    /// </summary>
    public CreateUserRequestValidator(IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(nameof(userRepository));

        RuleFor(u => u.FirstName).ValidFirstName();
        RuleFor(u => u.LastName).ValidFirstName();
        RuleFor(u => u.Email).ValidEmail(userRepository);
    }
}
