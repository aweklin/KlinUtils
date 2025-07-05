using KlinUtils.Common.Helpers.Results;

namespace KlinUtils.Common.Tests.Unit.Helpers.Results;

public class FailureTests
{
    [Fact]
    public void Constructor_WithSingleErrorInfo_StoresError()
    {
        // Given
        ErrorInfo error = ErrorInfo.BadRequest("Missing ID");

        // When
        Failure failure = new([error]);

        // Then
        Assert.Single(failure.Errors);
        Assert.Equal("BadRequest", failure.Errors.First().Code);
    }

    [Fact]
    public void ImplicitConversion_FromErrorInfo_CreatesFailure()
    {
        // Given
        ErrorInfo error = ErrorInfo.Conflict("Already exists");

        // When
        Failure failure = error;

        // Then
        Assert.Single(failure.Errors);
        Assert.Equal("Conflict", failure.Errors.First().Code);
        Assert.Equal("Already exists", failure.Errors.First().Message);
    }

    [Fact]
    public void FromErrorInfo_CreatesFailureCorrectly()
    {
        // Given
        ErrorInfo error = ErrorInfo.NotFound("Item not found");

        // When
        Failure failure = Failure.FromErrorInfo(error);

        // Then
        Assert.Single(failure.Errors);
        Assert.Equal("NotFound", failure.Errors.First().Code);
    }

    [Fact]
    public void FromMe_ReturnsNewFailureWithSameErrors()
    {
        // Given
        ErrorInfo error = ErrorInfo.Unauthorized();
        Failure original = new([error]);

        // When
        Failure copied = Failure.FromMe(original);

        // Then
        Assert.Single(copied.Errors);
        Assert.Equal("Unauthorized", copied.Errors.First().Code);
    }

    [Fact]
    public void FromMe_NullInput_ThrowsArgumentNullException()
    {
        // Given
        Failure? original = null;

        // When + Then
        Assert.Throws<ArgumentNullException>(() =>
        {
            Failure.FromMe(original!);
        });
    }
}