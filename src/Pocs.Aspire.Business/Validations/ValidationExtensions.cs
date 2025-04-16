

namespace Pocs.Aspire.Business.Validations;
using FluentValidation;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;
using System;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, Guid> ValidUserId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Id is required and must be a valid guid format.");
    }
    public static IRuleBuilderOptions<T, string> ValidFirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
    }
    public static IRuleBuilderOptions<T, string> ValidLastName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
    }

    public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder, IUserRepository userRepository)
    {
        return ruleBuilder
            .EmailAddress().WithMessage("A valid email is required.")
            .MaximumLength(320).WithMessage("Emails cannot exceed 320 characters.");
    }
}
