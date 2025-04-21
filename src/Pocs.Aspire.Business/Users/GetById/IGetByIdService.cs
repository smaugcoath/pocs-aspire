namespace Pocs.Aspire.Business.Users.GetById;

using LanguageExt;
using Pocs.Aspire.Domain.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

public interface IGetByIdService
{
    Task<Either<Failure, GetByIdResponse>> GetByIdAsync(GetByIdRequest request, CancellationToken cancellationToken = default);

}

/// <summary>
/// Represents the response returned when retrieving a user's details.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="FirstName">The first name of the user.</param>
/// <param name="LastName">The last name of the user.</param>
/// <param name="Email">The email address of the user.</param>
public record GetByIdResponse(Guid Id, string FirstName, string LastName, string Email);


/// <summary>
/// Represents the request data needed to retrieve a user's details.
/// </summary>
/// <param name="Id">The unique identifier of the user to retrieve.</param>
public record GetByIdRequest(Guid Id);