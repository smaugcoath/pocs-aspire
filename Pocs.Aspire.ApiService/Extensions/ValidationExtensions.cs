using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Pocs.Aspire.Business;
using System.Linq;

namespace Pocs.Aspire.ApiService.Extensions;

/// <summary>
/// Provides extension methods to customize ProblemDetails.
/// </summary>
public static class ProblemDetailsExtensions
{
    /// <summary>
    /// Adds custom properties (such as request method, path, requestId, and traceId) to the ProblemDetails.
    /// </summary>
    /// <param name="problemDetails">The ProblemDetails instance.</param>
    /// <param name="httpContext">The current HttpContext.</param>
    /// <returns>The modified ProblemDetails instance.</returns>
    public static ProblemDetails WithCustomProperties(this ProblemDetails problemDetails, HttpContext httpContext)
    {
        problemDetails.Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}";
        problemDetails.Extensions["requestId"] = httpContext.TraceIdentifier;
        var activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        problemDetails.Extensions["traceId"] = activity?.Id;
        return problemDetails;
    }

    /// <summary>
    /// Converts a ValidationError into a ProblemDetails instance with custom properties.
    /// </summary>
    /// <param name="error">The validation error containing FluentValidation errors.</param>
    /// <param name="httpContext">The current HttpContext.</param>
    /// <returns>A ProblemDetails instance representing the validation errors.</returns>
    public static ProblemDetails ToProblemDetails(this ValidationError error, HttpContext httpContext)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "Validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "Please see the errors property for details.",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["errors"] = error.Errors.Select(e => new
        {
            Field = e.PropertyName,
            Message = e.ErrorMessage
        });

        problemDetails = problemDetails.WithCustomProperties(httpContext);

        return problemDetails;
    }
}


