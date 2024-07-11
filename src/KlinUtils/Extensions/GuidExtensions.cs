using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace KlinUtils.Extensions;

// Inspired by Nick Chapsas in his youtube video: https://www.youtube.com/watch?v=B2yOjLyEZk0
public static class GuidExtensions
{
    private const char _hyphen = '-';
    private const char _slash = '/';
    private const byte _slashByte = (byte)'/';
    private const char _equals = '=';
    private const char _underscore = '_';
    private const char _plus = '+';
    private const byte _plusByte = (byte)'+';

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
                _hyphen => _slash,
                _underscore => _plus,
                _ => guid[i],
            };
        }

        base64Characters[22] = _equals;
        base64Characters[23] = _equals;

        Span<byte> guidBytes = stackalloc byte[16];
        Convert.TryFromBase64Chars(base64Characters, guidBytes, out _);

        return new Guid(guidBytes);
    }

    public static string Shorten(this in Guid guid)
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
                _slashByte => _hyphen,
                _plusByte => _underscore,
                _ => (char)base64Bytes[i],
            };
        }

        return new string(base64Characters);
    }
}