using System;
using System.Net.Mail;

namespace Pocs.Aspire.Domain.Users.ValueObjects;

/// <summary>
/// Represents an email address in the user domain.
/// </summary>
/// <remarks>
/// This value object encapsulates a valid email string and ensures it is structurally valid upon creation.
/// </remarks>
public readonly record struct Email
{
    public string Value { get; init; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email From(string value)
    {
        if (!MailAddress.TryCreate(value, out _))
        {
            throw new ArgumentException("The email address must be a valid email.", nameof(value));
        }

        return new Email(value);
    }
    /// <inheritdoc/>
    public override string ToString() => Value.ToString();

    public static implicit operator string(Email value) => value.Value;
}