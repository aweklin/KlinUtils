using FluentAssertions;

using KlinUtils.Extensions;

namespace KlinUtils.UnitTests.Extensions;

public class GuidExtensionsTests
{
    private static readonly Guid _guid = Guid.Parse("6de9e652-16f0-48f7-9d1e-7b321ad6069b");
    private static readonly string _guidConvertedString = "UubpbfAW90idHnsyGtYGmw";

    [Fact]
    public void Shorten_ShouldReturnEmptyString_WhenEmptyGuidIsProvided()
    {
        // given
        Guid inputGuid = Guid.Empty;
        string expectedStringRepresentation = string.Empty;

        // when
        string actualStringRepresentation = inputGuid.Shorten();

        // then
        actualStringRepresentation.Should().BeEquivalentTo(expectedStringRepresentation);
    }

    [Fact]
    public void Shorten_ShouldReturnGuidStringRepresentation_WhenAValidGuidIsProvided()
    {
        // given
        Guid inputGuid = _guid;
        string expectedStringRepresentation = _guidConvertedString;

        // when
        string actualStringRepresentation = inputGuid.Shorten();

        // then
        actualStringRepresentation.Should().BeEquivalentTo(expectedStringRepresentation);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void FromString_ShouldReturnEmpty_WhenEmptyStringRepresentationIsProvided(string inputStringRepresentation)
    {
        // given
        Guid expectedGuid = Guid.Empty;

        // when
        Guid actualGuid = inputStringRepresentation.FromString();

        // then
        actualGuid.Should().Be(expectedGuid);
    }

    [Fact]
    public void FromString_ShouldReturnGuid_WhenAValidStringRepresentationIsProvided()
    {
        // given
        string inputStringRepresentation = _guidConvertedString;
        Guid expectedGuid = _guid;

        // when
        Guid actualGuid = inputStringRepresentation.FromString();

        // then
        actualGuid.Should().Be(expectedGuid);
    }
}