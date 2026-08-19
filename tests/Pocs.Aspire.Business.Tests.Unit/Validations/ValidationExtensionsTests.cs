namespace Pocs.Aspire.Tests.Business.Tests.Unit.Validations;

using FluentValidation;
using Pocs.Aspire.Business.Validations;
using Shouldly;

public class ValidationExtensionsTests
{
    private sealed record EmailModel(string Email);

    // Regression test: the DB column for Email is varchar(100) (see UserConfiguration),
    // so validation must reject anything the database itself would reject.
    [Fact]
    public void ValidEmail_ShouldReject_WhenLongerThan100Characters()
    {
        var validator = new InlineValidator<EmailModel>();
        validator.RuleFor(x => x.Email).ValidEmail();
        var email = $"{new string('a', 90)}@example.com";

        var result = validator.Validate(new EmailModel(email));

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.ErrorMessage == "Emails cannot exceed 100 characters.");
    }

    [Fact]
    public void ValidEmail_ShouldAccept_WhenExactly100Characters()
    {
        var validator = new InlineValidator<EmailModel>();
        validator.RuleFor(x => x.Email).ValidEmail();
        var email = $"{new string('a', 88)}@example.com";
        email.Length.ShouldBe(100);

        var result = validator.Validate(new EmailModel(email));

        result.IsValid.ShouldBeTrue();
    }
}
