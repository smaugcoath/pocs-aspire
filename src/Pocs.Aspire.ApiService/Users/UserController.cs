using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Pocs.Aspire.ApiService.Extensions;
using Pocs.Aspire.Business.Users.Create;
using Pocs.Aspire.Business.Users.GetById;
using Pocs.Aspire.Business.Users.Update;
using Pocs.Aspire.Domain.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pocs.Aspire.ApiService.Users;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ICreateService _createService;
    private readonly IUpdateService _updateService;
    private readonly IGetByIdService _getByIdService;

    public UserController(ICreateService userService, IUpdateService updateService, IGetByIdService getByIdService)
    {
        _createService = userService;
        _updateService = updateService;
        _getByIdService = getByIdService;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// 
    [HttpPost]
    public async Task<Results<CreatedAtRoute<CreateResponse>, ValidationProblem>> Create([FromBody] CreateRequest request, CancellationToken cancellationToken)
    {
        var result = await _createService.CreateAsync(request, cancellationToken);

        return result.Case switch
        {
            CreateResponse response => TypedResults.CreatedAtRoute(response, nameof(GetById), new { id = response.Id }),
            ValidationError error => error.ToValidationProblem(HttpContext),
            _ => throw new NotImplementedException()
        };

    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<Results<Ok<UpdateResponse>, ValidationProblem, Conflict<EmailAlreadyExistsError>>> Update([FromRoute] Guid id, [FromBody] UpdateRequest request, CancellationToken cancellationToken)
    {
        request = request with { Id = id };

        var result = await _updateService.UpdateAsync(request, cancellationToken);

        return result.Case switch
        {
            UpdateResponse response =>  TypedResults.Ok(response),
            ValidationError error => error.ToValidationProblem(HttpContext), 
            EmailAlreadyExistsError error => TypedResults.Conflict(error),
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    [OutputCache(Duration = 5, VaryByQueryKeys = ["id"])]
    [HttpGet("{id:guid}", Name = nameof(GetById))]
    public async Task<Results<Ok<GetByIdResponse>, NotFound<NotFoundError>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        GetByIdRequest request = new(id);
        var result = await _getByIdService.GetByIdAsync(request, cancellationToken);

        return result.Case switch
        {
            GetByIdResponse response => TypedResults.Ok(response),
            NotFoundError error => TypedResults.NotFound(error),
            _ => throw new NotImplementedException()
        };
    }
}
