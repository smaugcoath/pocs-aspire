using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.Routing;
using Pocs.Aspire.ApiService.Extensions;
using Pocs.Aspire.Business.Users.Create;
using Pocs.Aspire.Business.Users.GetById;
using Pocs.Aspire.Business.Users.Update;
using Pocs.Aspire.Domain.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pocs.Aspire.ApiService.Users;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/users");

        group.MapPost("", Create)
            .ProducesProblem(StatusCodes.Status409Conflict);
        group.MapPut("{id:guid}", Update)
            .ProducesProblem(StatusCodes.Status409Conflict);
        group.MapGet("{id:guid}", GetById)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName(nameof(GetById));

        return builder;

    }
    public static async Task<Results<CreatedAtRoute<CreateResponse>, ValidationProblem, ProblemHttpResult>> Create(
        CreateRequest request,
        ICreateService createService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await createService.CreateAsync(request, cancellationToken);

        return result.Case switch
        {
            CreateResponse response => TypedResults.CreatedAtRoute(response, nameof(GetById), new { id = response.Id }),
            ValidationError error => error.ToValidationProblem(httpContext),
            EmailAlreadyExistsError error => TypedResults.Problem(title: error.Message, detail: error.Code, statusCode: StatusCodes.Status409Conflict),
            _ => throw new NotImplementedException()
        };

    }

    public static async Task<Results<Ok<UpdateResponse>, ValidationProblem, ProblemHttpResult>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateRequest request,
        IUpdateService updateService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        request = request with { Id = id };

        var result = await updateService.UpdateAsync(request, cancellationToken);

        return result.Case switch
        {
            UpdateResponse response => TypedResults.Ok(response),
            ValidationError error => error.ToValidationProblem(httpContext),
            EmailAlreadyExistsError error => TypedResults.Problem(title: error.Message, detail: error.Code, statusCode: StatusCodes.Status409Conflict),
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    [OutputCache(Duration = 5, VaryByQueryKeys = ["id"])]
    [HttpGet("{id:guid}", Name = nameof(GetById))]
    public static async Task<Results<Ok<GetByIdResponse>, ProblemHttpResult>> GetById(
        [FromRoute] Guid id,
        IGetByIdService getByIdService,
        CancellationToken cancellationToken)
    {
        GetByIdRequest request = new(id);
        var result = await getByIdService.GetByIdAsync(request, cancellationToken);

        return result.Case switch
        {
            GetByIdResponse response => TypedResults.Ok(response),
            NotFoundError error => TypedResults.Problem(title: error.Message, detail: error.Code, statusCode: StatusCodes.Status404NotFound),
            _ => throw new NotImplementedException()
        };
    }

}