namespace Pocs.Aspire.Domain.Users.ValueObjects;

/// <summary>
/// Represents a strongly-typed last name for a <see cref="User"/> entity.
/// </summary>
/// <remarks>
/// This value object encapsulates a valid last name string and ensures it is neither null nor improperly formatted.
/// Use <see cref="Create(string)"/> to build a validated instance.
/// </remarks>
public readonly record struct LastName
{
    /// <summary>
    /// Gets the raw string value of the last name.
    /// </summary>
    public string Value { get; }

    private LastName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new validated <see cref="LastName"/> instance.
    /// </summary>
    /// <param name="value">The raw last name string to validate and wrap.</param>
    /// <returns>A validated <see cref="LastName"/> value object.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the last name is null, empty, or not in a valid format.
    /// </exception>
    public static LastName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The last name cannot be empty.", nameof(value));
        }

        return new LastName(value);
    }

    /// <inheritdoc />
    public override string ToString() => Value;
}
