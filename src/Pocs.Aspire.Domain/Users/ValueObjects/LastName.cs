using System;

namespace Pocs.Aspire.Domain.Users.ValueObjects;

/// <summary>
/// Represents a strongly-typed last name for a <see cref="User"/> entity.
/// </summary>
/// <remarks>
/// This value object encapsulates a valid last name string and ensures it is neither null nor improperly formatted.
/// </remarks>
public readonly record struct LastName
{
    public string Value { get; init; }

    private LastName(string value)
    {
        Value = value;
    }

    public static LastName From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The last name cannot be empty.", nameof(value));
        }

        return new LastName(value);
    }
    /// <inheritdoc/>
    public override string ToString() => Value.ToString();

    public static implicit operator string(LastName value) => value.Value;
}
