using System;

namespace Pocs.Aspire.Domain.Users.ValueObjects;

/// <summary>
/// Represents a strongly-typed first name for an <see cref="User"/> entity.
/// </summary>
/// <remarks>
/// This value object encapsulates a valid first name string and ensures it is neither null nor improperly formatted.
/// </remarks>
public readonly record struct FirstName
{
    public string Value { get; init; }

    private FirstName(string value)
    {
        Value = value;
    }

    public static FirstName From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The first name cannot be empty.", nameof(value));
        }

        return new FirstName(value);
    }
    /// <inheritdoc/>
    public override string ToString() => Value.ToString();

    public static implicit operator string(FirstName value) => value.Value;

}
