using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Pocs.Aspire.Domain.Errors;
using System;
using System.Linq;

namespace Pocs.Aspire.ApiService.Extensions;
/// <summary>
/// Provides extension methods to customize ProblemDetails.
/// </summary>
public static class ProblemDetailsExtensions
{
    /// <summary>
    /// Converts a ValidationError into a ProblemDetails instance with custom properties.
    /// </summary>
    /// <param name="exception">The validation error containing domain errors.</param>
    /// <param name="httpContext">The current HttpContext.</param>
    /// <returns>A ProblemDetails instance representing the validation errors.</returns>
    public static ValidationProblem ToValidationProblem(this ValidationError error, HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(error);
        ArgumentNullException.ThrowIfNull(httpContext);


        var errors = error.Errors
            .GroupBy(e => e.Field)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Message).ToArray());

        return TypedResults.ValidationProblem(
            errors,
            detail: $"{errors.Count} validation error(s) occurred.",
            instance: $"{httpContext.Request.Method} {httpContext.Request.Path}",
            title: "Validation error"
        );
    }
}


