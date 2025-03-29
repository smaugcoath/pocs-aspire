using Microsoft.AspNetCore.Mvc;
using System;

namespace Pocs.Aspire.ApiService.Users;

/// <summary>
/// Contains all request and response models for the User endpoints.
/// </summary>
public static class Models
{
    /// <summary>
    /// Represents the response returned after creating a new user.
    /// </summary>
    /// <param name="id">The unique identifier of the created user.</param>
    public record CreateUserResponse(Guid id);

    /// <summary>
    /// Represents the request data needed to create a new user.
    /// </summary>
    /// <param name="FirstName">The first name of the user.</param>
    /// <param name="LastName">The last name of the user.</param>
    /// <param name="Email">The email address of the user.</param>
    public record CreateUserRequest(string FirstName, string LastName, string Email);

    /// <summary>
    /// Represents the response returned when retrieving a user's details.
    /// </summary>
    /// <param name="Id">The unique identifier of the user.</param>
    /// <param name="FirstName">The first name of the user.</param>
    /// <param name="LastName">The last name of the user.</param>
    /// <param name="Email">The email address of the user.</param>
    public record GetUserResponse(Guid Id, string FirstName, string LastName, string Email);

    
    // Removed, this works fine. But swagger can't handle it. Removed for simplicity, I'm just experimeting with the binding system.
    /// <summary>
    /// Represents the request data needed to retrieve a user's details.
    /// </summary>
    /// <param name="Id">The unique identifier of the user to retrieve.</param>
    //public record GetUserRequest([property: FromRoute] Guid Id);


    /// <summary>
    /// Represents the response returned after updating a user's details.
    /// </summary>
    /// <param name="Id">The unique identifier of the updated user.</param>
    /// <param name="FirstName">The updated first name of the user.</param>
    /// <param name="LastName">The updated last name of the user.</param>
    /// <param name="Email">The updated email address of the user.</param>
    public record UpdateUserResponse(Guid Id, string FirstName, string LastName, string Email);

    /// <summary>
    /// Represents the request data needed to update an existing user.
    /// </summary>
    /// <param name="FirstName">The new first name of the user.</param>
    /// <param name="LastName">The new last name of the user.</param>
    /// <param name="Email">The new email address of the user.</param>
    public record UpdateUserRequest(string FirstName, string LastName, string Email);
}
