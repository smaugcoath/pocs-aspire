using FluentValidation;
using Pocs.Aspire.Business.Validations;

namespace Pocs.Aspire.Business.Users.Update;
public class UpdateRequestValidator : AbstractValidator<UpdateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRequestValidator"/> class.
    /// </summary>
    public UpdateRequestValidator()
    {
        RuleFor(x => x.Id).ValidUserId();
        RuleFor(u => u.FirstName).ValidFirstName();
        RuleFor(u => u.LastName).ValidFirstName();
        RuleFor(u => u.Email).ValidEmail();
    }
}
