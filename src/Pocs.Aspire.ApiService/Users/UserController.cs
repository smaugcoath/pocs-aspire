using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Pocs.Aspire.ApiService.Extensions;
using Pocs.Aspire.Business.Users.Create;
using Pocs.Aspire.Business.Users.GetById;
using Pocs.Aspire.Business.Users.Update;
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
    [ProducesResponseType<CreateResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Create([FromBody] CreateRequest request, CancellationToken cancellationToken)
    {
        var result = await _createService.CreateAsync(request, cancellationToken);

        return result.Case switch
        {
            CreateResponse response => Results.CreatedAtRoute(nameof(Get), new { id = response.Id }, response),
            Exception error => error.ToProblem(HttpContext),
            _ => throw new NotImplementedException()
        };

    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<UpdateResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Update([FromRoute] Guid id, [FromBody] UpdateRequest request, CancellationToken cancellationToken)
    {
        request = request with { Id = id };

        var result = await _updateService.UpdateAsync(request, cancellationToken);

        return await result.Match
            (
                response => Results.Ok(response),
                error => error.ToProblem(HttpContext)
            );
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    [OutputCache(Duration = 5, VaryByQueryKeys = ["id"])]
    [HttpGet("{id:guid}", Name = nameof(Get))]
    [ProducesResponseType<UpdateResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        GetByIdRequest request = new(id);
        var result = await _getByIdService.GetByIdAsync(request, cancellationToken);

        return result.Case switch
        {
            GetByIdResponse response => Results.Ok(response),
            Exception error => error.ToProblem(HttpContext),
            _ => throw new NotImplementedException()
        };
    }
}
