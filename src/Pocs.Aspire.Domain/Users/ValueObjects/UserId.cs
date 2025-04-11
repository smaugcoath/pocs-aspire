using System;

namespace Pocs.Aspire.Domain.Users.ValueObjects;

/// <summary>
/// Represents a strongly-typed unique identifier for a <see cref="User"/> entity.
/// </summary>
/// <remarks>
/// <para>
/// This value object enforces the invariant that a <see cref="UserId"/> can never be <see cref="Guid.Empty"/>.
/// It encapsulates the raw <see cref="Guid"/> to eliminate primitive obsession and preserve domain integrity.
/// </para>
/// <para>
/// Use <see cref="New"/> to generate a new identifier, or <see cref="From(Guid)"/> to construct one from an existing <see cref="Guid"/>.
/// </para>
/// </remarks>
public readonly record struct UserId
{
    public Guid Value { get; init; }
    private UserId(Guid value)
    {
        Value = value;
    }

    public static UserId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("The user identifier cannot be empty.", nameof(value));
        }

        return new UserId(value);
    }

    public static UserId New() => From(Guid.NewGuid());

    /// <inheritdoc/>
    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserId value) => value.Value;
}