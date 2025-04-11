using FluentValidation;
using Pocs.Aspire.Business.Validations;

namespace Pocs.Aspire.Business.Users.Create;
public class CreateUserRequestValidator : AbstractValidator<CreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserRequestValidator"/> class.
    /// </summary>
    public CreateUserRequestValidator()
    {
        RuleFor(u => u.FirstName).ValidFirstName();
        RuleFor(u => u.LastName).ValidFirstName();
        RuleFor(u => u.Email).ValidEmail();
    }
}
