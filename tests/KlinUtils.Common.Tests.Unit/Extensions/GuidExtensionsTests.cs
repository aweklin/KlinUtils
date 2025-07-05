using KlinUtils.Common.Extensions;

namespace KlinUtils.Common.Tests.Unit.Extensions;

public class GuidExtensionsTests
{
    [Fact]
    public void Shorten_EmptyGuid_ReturnsEmptyString()
    {
        // Given
        Guid input = Guid.Empty;

        // When
        string shortGuid = input.Shorten();

        // Then
        Assert.Equal(string.Empty, shortGuid);
    }

    [Fact]
    public void Shorten_ValidGuid_Returns22CharacterString()
    {
        // Given
        Guid guid = Guid.NewGuid();

        // When
        string shortGuid = guid.Shorten();

        // Then
        Assert.Equal(22, shortGuid.Length);
    }

    [Fact]
    public void ShortenAndFromString_RoundTrip_ReturnsOriginalGuid()
    {
        // Given
        Guid original = Guid.NewGuid();

        // When
        string shortGuid = original.Shorten();
        Guid parsed = shortGuid.FromString();

        // Then
        Assert.Equal(original, parsed);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void FromString_NullOrWhitespace_ReturnsEmptyGuid(string? input)
    {
        // Given

        // When
        Guid result = input!.FromString();

        // Then
        Assert.Equal(Guid.Empty, result);
    }

    [Fact]
    public void FromString_ValidShortGuid_ReconstructsExpectedGuid()
    {
        // Given
        Guid knownGuid = Guid.NewGuid();
        string shortString = knownGuid.Shorten();

        // When
        Guid reconstructed = shortString.FromString();

        // Then
        Assert.Equal(knownGuid, reconstructed);
    }

    [Fact]
    public void Shorten_ReplacesSlashWithHyphen_AndPlusWithUnderscore()
    {
        // Given
        Guid guid = Guid.Parse("00112233-4455-6677-8899-AABBCCDDEEFF");

        // When
        string shortGuid = guid.Shorten();

        // Then
        bool containsHyphenOrUnderscore = shortGuid.Any(c => c is '-' or '_');
        Assert.True(containsHyphenOrUnderscore);
    }

    [Fact]
    public void FromString_UnderscoreAndHyphen_MapsCorrectlyBackToOriginal()
    {
        // Given
        Guid guid = Guid.Parse("12345678-9abc-def0-1234-56789abcdef0");
        string shortened = guid.Shorten();

        // Sanity check: ensure it has transformation chars
        Assert.False(shortened.Contains('-', StringComparison.CurrentCultureIgnoreCase));
        Assert.False(shortened.Contains('_', StringComparison.CurrentCultureIgnoreCase));

        // When
        Guid parsed = shortened.FromString();

        // Then
        Assert.Equal(guid, parsed);
    }

    [Fact]
    public void FromString_WithInvalidLength_ThrowsFormatException()
    {
        // Given
        string invalid = "TooShortString!";

        // When + Then
        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            invalid.FromString();
        });
    }
}