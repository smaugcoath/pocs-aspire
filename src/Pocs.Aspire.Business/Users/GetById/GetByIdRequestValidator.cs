using FluentValidation;
using Pocs.Aspire.Business.Validations;

namespace Pocs.Aspire.Business.Users.GetById;
public class CreateUserRequestValidator : AbstractValidator<GetByIdRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserRequestValidator"/> class.
    /// </summary>
    public CreateUserRequestValidator()
    {
        RuleFor(u => u.Id).ValidUserId();
    }
}
