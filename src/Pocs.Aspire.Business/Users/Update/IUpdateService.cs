using LanguageExt;
using Pocs.Aspire.Domain.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pocs.Aspire.Business.Users.Update;

public interface IUpdateService
{
    Task<Either<Error, UpdateResponse>> UpdateAsync(UpdateRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents the request data needed to update an existing user.
/// </summary>
/// <param name="FirstName">The new first name of the user.</param>
/// <param name="LastName">The new last name of the user.</param>
/// <param name="Email">The new email address of the user.</param>
public record UpdateRequest(Guid Id, string FirstName, string LastName, string Email);

/// <summary>
/// Represents the response returned after updating a user's details.
/// </summary>
/// <param name="Id">The unique identifier of the updated user.</param>
/// <param name="FirstName">The updated first name of the user.</param>
/// <param name="LastName">The updated last name of the user.</param>
/// <param name="Email">The updated email address of the user.</param>
public record UpdateResponse(Guid Id, string FirstName, string LastName, string Email);
