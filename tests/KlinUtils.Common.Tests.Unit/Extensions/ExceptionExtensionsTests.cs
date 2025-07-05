using KlinUtils.Common.Extensions;

namespace KlinUtils.Common.Tests.Unit.Extensions;

public class ExceptionExtensionsTests
{
    [Fact]
    public void GetDetails_NullException_ThrowsArgumentNullException()
    {
        // Given
        Exception? exception = null;

        // When + Then
        Assert.Throws<ArgumentNullException>(() =>
        {
            exception!.GetDetails();
        });
    }

    [Fact]
    public void GetDetails_ExceptionWithoutInnerMessage_ReturnsOriginalMessage()
    {
        // Given
        string message = "Something went wrong";
#pragma warning disable CA2201 // Do not raise reserved exception types
        Exception exception = new(message);
#pragma warning restore CA2201 // Do not raise reserved exception types

        // When
        string result = exception.GetDetails();

        // Then
        Assert.Equal(message, result);
    }

    [Fact]
    public void GetDetails_SingleLevelWithInnerMessage_ReturnsInner()
    {
        // Given
#pragma warning disable CA2201 // Do not raise reserved exception types
        Exception inner = new("Actual cause");
        Exception outer = new("See inner exception for details", inner);
#pragma warning restore CA2201 // Do not raise reserved exception types

        // When
        string result = outer.GetDetails();

        // Then
        Assert.Equal("Actual cause", result);
    }

    [Fact]
    public void GetDetails_DeepNestedInnerException_ReturnsInnermostMessage()
    {
        // Given
#pragma warning disable CA2201 // Do not raise reserved exception types
        Exception deepest = new("Root cause of failure");
        Exception level2 = new("See inner exception", deepest);
        Exception level1 = new("see INNER exception", level2); // Intentional case variant
        Exception top = new("SEE INNER EXCEPTION", level1);
#pragma warning restore CA2201 // Do not raise reserved exception types

        // When
        string result = top.GetDetails();

        // Then
        Assert.Equal("Root cause of failure", result);
    }

    [Fact]
    public void GetDetails_MessageContainsButIsNotExactlyIndicator_ReturnsThatMessage()
    {
        // Given
        string message = "An error occurred: see inner exception not required.";
#pragma warning disable CA2201 // Do not raise reserved exception types
        Exception exception = new(message);
#pragma warning restore CA2201 // Do not raise reserved exception types

        // When
        string result = exception.GetDetails();

        // Then
        Assert.Equal(message, result);
    }
}