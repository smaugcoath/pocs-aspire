using FluentValidation;

namespace Pocs.Aspire.Business.User;

using Pocs.Aspire.Domain.Users;
public class UserValidator: AbstractValidator<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserValidator"/> class.
    /// </summary>
    public UserValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
    }
}
