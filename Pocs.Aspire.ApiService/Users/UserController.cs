using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Pocs.Aspire.ApiService.Extensions;
using Pocs.Aspire.ApiService.Mappers;
using Pocs.Aspire.Business.Users;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Pocs.Aspire.ApiService.Users.Models;

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
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = request.ToDomain();
        var result = await _userService.CreateAsync(user, cancellationToken);

        return result.Match<IActionResult>(
              success => CreatedAtAction(nameof(Get), new { id = success.Id }, success.ToCreateUserResponse()),
              error => BadRequest(error.ToProblemDetails(HttpContext)));
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        // TODO create mapper
        var updated = request.ToDomain(id);
        var result = await _userService.UpdateAsync(updated, cancellationToken);

        // TODO Create mappers
        return result.Match<IActionResult>(
            success => Ok(success.ToUpdateUserResponse()),
            error => NotFound(),
            error => BadRequest(error.ToProblemDetails(HttpContext)));
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    [OutputCache(Duration = 5, VaryByQueryKeys = ["id"])]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _userService.GetAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            success => Ok(success.ToGetUserResponse()),
            _ => NotFound());
    }
}
