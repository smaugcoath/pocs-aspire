namespace Pocs.Aspire.Domain.Users.ValueObjects;

/// <summary>
/// Represents a strongly-typed first name for a <see cref="User"/> entity.
/// </summary>
/// <remarks>
/// This value object encapsulates a valid first name string and ensures it is neither null nor improperly formatted.
/// Use <see cref="Create(string)"/> to build a validated instance.
/// </remarks>
public readonly record struct FirstName
{
    /// <summary>
    /// Gets the raw string value of the first name.
    /// </summary>
    public string Value { get; }

    private FirstName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new validated <see cref="FirstName"/> instance.
    /// </summary>
    /// <param name="value">The raw first name string to validate and wrap.</param>
    /// <returns>A validated <see cref="FirstName"/> value object.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the first name is null, empty, or not in a valid format.
    /// </exception>
    public static FirstName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The first name cannot be empty.", nameof(value));
        }

        return new FirstName(value);
    }

    /// <inheritdoc />
    public override string ToString() => Value;
}
