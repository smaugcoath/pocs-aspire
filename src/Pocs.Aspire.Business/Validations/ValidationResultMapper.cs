using FluentValidation.Results;
using Pocs.Aspire.Domain.Errors;
using System.Collections.Generic;
using System.Linq;

namespace Pocs.Aspire.Business.Validations;

public static class ValidationResultMapper
{
    public static IEnumerable<FieldError> ToFieldErrors(this ValidationResult result) =>
        result.Errors.Select(e => new FieldError(e.PropertyName, e.ErrorMessage));
}
