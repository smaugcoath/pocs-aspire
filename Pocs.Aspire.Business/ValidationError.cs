using FluentValidation.Results;
using System.Collections.Generic;

namespace Pocs.Aspire.Business;

public record struct ValidationError(IEnumerable<ValidationFailure> Errors)
{
    public ValidationError(ValidationFailure error) : this([error])
    {
    }

}
