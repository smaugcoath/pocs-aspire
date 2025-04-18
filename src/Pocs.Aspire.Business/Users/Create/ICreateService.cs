namespace Pocs.Aspire.Business.Users.Create;

using LanguageExt;
using Pocs.Aspire.Domain.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

public interface ICreateService
{
    Task<Either<Failure, CreateResponse>> CreateAsync(CreateRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents the request data needed to create a new user.
/// </summary>
/// <param name="FirstName">The first name of the user.</param>
/// <param name="LastName">The last name of the user.</param>
/// <param name="Email">The email address of the user.</param>
public record CreateRequest(string FirstName, string LastName, string Email);
/// <summary>
/// Represents the response returned after creating a new user.
/// </summary>
/// <param name="Id">The unique identifier of the created user.</param>
public record CreateResponse(Guid Id);