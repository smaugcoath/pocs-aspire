﻿using Pocs.Aspire.Domain.Users.ValueObjects;

namespace Pocs.Aspire.Domain.Users;

/// <summary>
/// Represents an user.
/// </summary>
/// <remarks>
/// This aggregate root includes identity and personal information for a user.
/// </remarks>
public class User
{
    /// <summary>
    /// Gets the unique identifier for the user.
    /// </summary>
    public UserId Id { get; init; }

    /// <summary>
    /// Gets the user's first name.
    /// </summary>
    public FirstName FirstName { get; set; }

    /// <summary>
    /// Gets the user's last name.
    /// </summary>
    public LastName LastName { get; set; }

    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    public Email Email { get; set; }

    private User(UserId id, FirstName firstName, LastName lastName, Email email) =>
            (Id, FirstName, LastName, Email) = (id, firstName, lastName, email);

    public static User From(UserId id, FirstName firstName, LastName lastName, Email email) =>
            new User(id, firstName, lastName, email);
    public static User New(FirstName firstName, LastName lastName, Email email) =>
        new User(UserId.New(), firstName, lastName, email);

}
