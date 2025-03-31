using FluentValidation.Results;
using Pocs.Aspire.Business.Users;
using System.Linq;

namespace Pocs.Aspire.Business.Validations;

public static class ValidationResultMapper
{
    public static BusinessValidationException ToValidationException(this ValidationResult result) =>
        new(result.Errors.Select(e => new FieldError(e.PropertyName, e.ErrorMessage)));
}
