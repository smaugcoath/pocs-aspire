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
/// Use <see cref="New"/> to generate a new identifier, or <see cref="Create(Guid)"/> to construct one from an existing <see cref="Guid"/>.
/// </para>
/// </remarks>
public readonly record struct UserId
{
    /// <summary>
    /// Gets the underlying <see cref="Guid"/> value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserId"/> struct with the specified <see cref="Guid"/>.
    /// </summary>
    /// <param name="value">A non-empty <see cref="Guid"/> representing the identifier.</param>
    /// <returns>A valid <see cref="UserId"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided <paramref name="value"/> is <see cref="Guid.Empty"/>.
    /// </exception>
    public static UserId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("The user identifier cannot be empty.", nameof(value));
        }

        return new UserId(value);
    }

    /// <summary>
    /// Generates a new <see cref="UserId"/> using a randomly created <see cref="Guid"/>.
    /// </summary>
    /// <returns>A new instance of <see cref="UserId"/> with a unique identifier.</returns>
    public static UserId New() => Create(Guid.NewGuid());

    private UserId(Guid value) => Value = value;

    /// <inheritdoc/>
    public override string ToString() => Value.ToString();
}
