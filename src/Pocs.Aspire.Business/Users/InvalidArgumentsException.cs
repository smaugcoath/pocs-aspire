using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pocs.Aspire.Business.Users;


public class BusinessValidationException : Exception
{
    public IReadOnlyCollection<FieldError> Errors { get; }

    public BusinessValidationException(IEnumerable<FieldError> errors)
        : base("Validation failed.")
    {
        Errors = errors.ToList().AsReadOnly();
    }
}

