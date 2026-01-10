namespace CVA.Domain.Models;

/// <summary>
/// Argument validation.
/// </summary>
internal static class Ensure
{
    /// <summary>
    /// Ensures that the specified argument is not null.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the argument, used in the exception if thrown.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
    public static void NotNull(object? value, string name)
    {
        if (value is not null) return;
        throw new ArgumentNullException(name);
    }

    /// <summary>
    /// Ensures that the specified argument is not empty.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the argument, used in the exception if thrown.</param>
    /// <exception cref="ArgumentException">Thrown when the value is empty.</exception>
    public static void NotEmpty(Guid value, string name)
    {
        if (value != Guid.Empty) return;
        throw new ArgumentException("Empty guid.", name);
    }

    /// <summary>
    /// Ensures that the specified string is not null or empty.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <param name="name">The name of the argument, used in the exception if thrown.</param>
    /// <returns>The trimmed string if not null or empty, otherwise throws an exception.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is null or empty.</exception>
    public static string TrimToNull(string? value, string name)
    {
        value = value?.Trim();
        return !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException("Empty value.", name);
    }
}