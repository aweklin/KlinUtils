using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace KlinUtils.Guards;

public static class Ensure
{
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