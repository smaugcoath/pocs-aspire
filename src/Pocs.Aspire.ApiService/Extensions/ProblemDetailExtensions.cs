using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Pocs.Aspire.ApiService.Validations;
using Pocs.Aspire.Business;
using Pocs.Aspire.Business.Users;
using System;
using System.Collections.Generic;
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
        problemDetails.Extensions.AddCustomExtensions(httpContext);
        return problemDetails;
    }

    public static IDictionary<string, object?> AddCustomExtensions(this IDictionary<string, object?> extensions, HttpContext httpContext)
    {
        extensions["requestId"] = httpContext.TraceIdentifier;
        var activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        extensions["traceId"] = activity?.Id;

        return extensions;
    }

    /// <summary>
    /// Converts a ValidationError into a ProblemDetails instance with custom properties.
    /// </summary>
    /// <param name="exception">The validation error containing FluentValidation errors.</param>
    /// <param name="httpContext">The current HttpContext.</param>
    /// <returns>A ProblemDetails instance representing the validation errors.</returns>
    public static IResult ToProblem(this Exception exception, HttpContext httpContext)
    {
        var problem = Results.Problem
            (
                detail: GetDetails(exception),
                instance: $"{httpContext.Request.Method} {httpContext.Request.Path}",
                statusCode: GetStatusCode(exception),
                title: GetTitle(exception),
                type: exception.GetType().Name,
                extensions: GetExtensions(exception)?.AddCustomExtensions(httpContext)
            );

        return problem;
    }

    private static string GetDetails(Exception exception)
         => exception switch
         {
             BusinessValidationException e => "See the 'errors' property for details.",
             NotFoundException e => "The resource does not exist.",
             _ => "Contact with the IT department."
         };
    private static int GetStatusCode(Exception exception)
         => exception switch
         {
             BusinessValidationException e => StatusCodes.Status400BadRequest,
             NotFoundException e => StatusCodes.Status404NotFound,
             _ => StatusCodes.Status500InternalServerError
         };

    private static string GetTitle(Exception exception)
        => exception switch
        {
            BusinessValidationException e => "Validation errors occurred.",
            NotFoundException e => "The resource does not exist.",
            _ => "Unexpected error."
        };

    private static IDictionary<string, object?>? GetExtensions(Exception exception)
    {
        var result = new Dictionary<string, object?>();

        if (exception is BusinessValidationException validationException)
        {
            var validationErrors = validationException.Errors.Select(e => new ValidationErrorModel(e.Field, e.Message));
            result.Add("errors", validationErrors);
        }

        return result;
    }
}


