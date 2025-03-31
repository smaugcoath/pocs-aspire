namespace Pocs.Aspire.ApiService.Mappers;

using Pocs.Aspire.ApiService.Users;
using Pocs.Aspire.Domain.Users;
using Pocs.Aspire.Domain.Users.ValueObjects;

/// <summary>
/// Provides extension methods for mapping between User domain entities and request/response DTOs.
/// </summary>
internal static class UserMapperExtensions
{
    /// <summary>
    /// Maps a CreateUserRequest to a domain User entity.
    /// </summary>
    /// <param name="request">The CreateUserRequest instance.</param>
    /// <returns>A new User entity.</returns>
    internal static User ToDomain(this CreateUserRequest request) =>
         new()
         {
             FirstName = request.FirstName,
             LastName = request.LastName,
             Email = request.Email
         };

    /// <summary>
    /// Maps a User domain entity to a CreateUserResponse.
    /// </summary>
    /// <param name="user">The User entity.</param>
    /// <returns>A CreateUserResponse with the user's Id.</returns>
    internal static CreateUserResponse ToCreateUserResponse(this User user) =>
        new CreateUserResponse(user.Id);

    /// <summary>
    /// Maps an UpdateUserRequest and a given UserId to a User domain entity.
    /// </summary>
    /// <param name="request">The UpdateUserRequest instance.</param>
    /// <param name="id">The UserId to use for the update.</param>
    /// <returns>An updated User entity.</returns>
    internal static User ToDomain(this UpdateUserRequest request, UserId id) =>
        new ()
        {
            Id = id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };

    /// <summary>
    /// Maps a User domain entity to an UpdateUserResponse.
    /// </summary>
    /// <param name="user">The User entity.</param>
    /// <returns>An UpdateUserResponse containing user details.</returns>
    internal static UpdateUserResponse ToUpdateUserResponse(this User user) =>
        new UpdateUserResponse(user.Id, user.FirstName, user.LastName, user.Email);

    /// <summary>
    /// Maps a User domain entity to a GetUserResponse.
    /// </summary>
    /// <param name="user">The User entity.</param>
    /// <returns>A GetUserResponse with user details.</returns>
    internal static GetUserResponse ToGetUserResponse(this User user) =>
        new GetUserResponse(user.Id, user.FirstName, user.LastName, user.Email);
}
