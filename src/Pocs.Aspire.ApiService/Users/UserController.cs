using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Pocs.Aspire.ApiService.Extensions;
using Pocs.Aspire.ApiService.Mappers;
using Pocs.Aspire.Business.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pocs.Aspire.ApiService.Users;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// 
    [HttpPost]
    [ProducesResponseType<CreateUserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Create(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = request.ToDomain();
        var result = await _userService.CreateAsync(user, cancellationToken);

        return result.Match(
              success => Results.CreatedAtRoute(nameof(Get), new { id = success.Id }, success.ToCreateUserResponse()),
              error => error.ToProblem(HttpContext));
              
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<UpdateUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var updated = request.ToDomain(id);
        var result = await _userService.UpdateAsync(updated, cancellationToken);

        return result.Match(
            success => Results.Ok(success.ToUpdateUserResponse()),
            error => error.ToProblem(HttpContext));
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    [OutputCache(Duration = 5, VaryByQueryKeys = ["id"])]
    [HttpGet("{id:guid}", Name = nameof(Get))]
    [ProducesResponseType<UpdateUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _userService.GetAsync(id, cancellationToken);

        return result.Match(
            success => Results.Ok(success.ToGetUserResponse()),
            error => error.ToProblem(HttpContext));
    }
}
