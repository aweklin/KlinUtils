using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace KlinUtils.Common.Extensions;

// Inspired by Nick Chapsas in his youtube video: https://www.youtube.com/watch?v=B2yOjLyEZk0
public static class GuidExtensions
{
    private const char Hyphen = '-';
    private const char Slash = '/';
    private const byte SlashByte = (byte)'/';
    private const char EEquals = '=';
    private const char Underscore = '_';
    private const char Plus = '+';
    private const byte PlusByte = (byte)'+';

    public static Guid FromString([NotNull] this string convertedGuid)
    {
        if (string.IsNullOrWhiteSpace(convertedGuid))
        {
            return Guid.Empty;
        }

        ReadOnlySpan<char> guid = convertedGuid;
        Span<char> base64Characters = stackalloc char[24];

        for (int i = 0; i < 22; i++)
        {
            base64Characters[i] = guid[i] switch
            {
                Hyphen => Slash,
                Underscore => Plus,
                _ => guid[i],
            };
        }

        base64Characters[22] = EEquals;
        base64Characters[23] = EEquals;

        Span<byte> guidBytes = stackalloc byte[16];
        Convert.TryFromBase64Chars(base64Characters, guidBytes, out _);

        return new Guid(guidBytes);
    }

#pragma warning disable CA1720 // Identifier contains type name
    public static string Shorten(this in Guid guid)
#pragma warning restore CA1720 // Identifier contains type name
    {
        if (guid == Guid.Empty)
        {
            return string.Empty;
        }

        Span<byte> guidBytes = stackalloc byte[16];
        Span<byte> base64Bytes = stackalloc byte[24];

        MemoryMarshal.TryWrite(guidBytes, guid);
        Base64.EncodeToUtf8(guidBytes, base64Bytes, out _, out _);

        Span<char> base64Characters = stackalloc char[22];

        for (int i = 0; i < 22; i++)
        {
            base64Characters[i] = base64Bytes[i] switch
            {
                SlashByte => Hyphen,
                PlusByte => Underscore,
                _ => (char)base64Bytes[i],
            };
        }

        return new string(base64Characters);
    }
}