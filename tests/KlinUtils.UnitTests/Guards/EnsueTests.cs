using FluentAssertions;

using KlinUtils.Guards;

namespace KlinUtils.UnitTests.Guards;

public class EnsueTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NotNullOrEmpty_ShouldThrowArgumentNullException_WhenArgumentIsActuallyNullOrEmpty(string? value)
    {
        Action action = () => Ensure.NotNullOrEmpty(value);

        FluentActions
            .Invoking(action)
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void NotNullOrEmpty_ShouldSucceed_WhenArgumentIsValid()
    {
        string? value = "somevalue";

        Ensure.NotNullOrEmpty(value);

        value.Should().NotBeEmpty();
    }
}