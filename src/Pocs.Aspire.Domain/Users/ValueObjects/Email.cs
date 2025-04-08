using System.Net.Mail;

namespace Pocs.Aspire.Domain.Users.ValueObjects;

/// <summary>
/// Represents an email address in the user domain.
/// </summary>
/// <remarks>
/// This value object encapsulates a valid email string and ensures it is neither null nor improperly formatted.
/// Use <see cref="Create(string)"/> to build a validated instance.
/// </remarks>
public readonly record struct Email
{
    /// <summary>
    /// Gets the raw string value of the email address.
    /// </summary>
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new validated <see cref="Email"/> instance.
    /// </summary>
    /// <param name="value">The raw email string to validate and wrap.</param>
    /// <returns>A validated <see cref="Email"/> value object.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the email is null, empty, or not in a valid format.
    /// </exception>
    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The email address cannot be empty.", nameof(value));
        }

        try
        {
            var address = new MailAddress(value);
            if (address.Address != value)
            {
                throw new ArgumentException("Invalid email format.", nameof(value));
            }
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid email format.", nameof(value));
        }

        return new Email(value);
    }

    /// <inheritdoc />
    public override string ToString() => Value;
}
