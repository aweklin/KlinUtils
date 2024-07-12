using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace KlinUtils.Guards;

/// <summary>
/// Contains definitions for guide clauses.
/// </summary>
public static class Ensure
{
    /// <summary>
    /// Ensures that a given string value is not null or empty.
    /// </summary>
    /// <param name="value">Nullable string object.</param>
    /// <param name="paramName">Specifies the parameter name.</param>
    /// <exception cref="ArgumentNullException">System.ArgumentNullException.</exception>
    public static void NotNullOrEmpty(
        [NotNull] string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(paramName);
        }
    }
}