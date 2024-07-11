using FluentAssertions;

using KlinUtils.DiscriminatedUnion;

namespace KlinUtils.UnitTests.UnitTests.DiscriminatedUnion;

public class ResultTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void WithMessage_Should_ThrowArgumentNullException_WhenErrorMessageSuppliedIsEmpty(string message)
    {
        // when
        Action comparison = () =>
        {
            Error error = Error.WithMessage(message);
            Result result = error;
        };

        // then
        comparison.Should().ThrowExactly<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Required_Should_ThrowArgumentNullException_WhenErrorMessageSuppliedIsEmpty(string message) // ReturnTrueForHasErrorProperty_WhenAnErrorIsPassed()
    {
        // when
        Action comparison = () =>
        {
            Error error = Error.Required(message);
            Result result = error;
        };

        // then
        comparison.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Result_Should_NotResultInError_WhenNoneIsPassedAsError()
    {
        // given
        Error none = Error.None;
        bool expectedResultForIsSuccessProperty = true;
        bool expectedResultForHasErrorProperty = false;

        // when
        Result result = none;

        // then
        result.IsSuccess.Should().Be(expectedResultForIsSuccessProperty);
        result.HasError.Should().Be(expectedResultForHasErrorProperty);
        result.Errors.Count.Should().Be(0);
    }

    [Fact]
    public void Failures_Should_ThrowInvalidOperationException_WhenErrorListIsEmpty()
    {
        // when
        Action comparison = () =>
        {
            Result result = Result.Failures([]);
        };

        // then
        comparison.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Failures_Should_ThrowInvalidOperationException_WhenNoneIsIncludedInTheErrorList()
    {
        // when
        Action comparison = () =>
        {
            Result result = Result.Failures(
            [
                Error.Required("First name"),
                Error.None,
                Error.Required("Last name"),
                Error.WithMessage("Unknown error occurred")
            ]);
        };

        // then
        comparison.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Failures_Should_ContainsUniqueErrorObjects_EvenIfADuplicateErrorObjectIsPassed()
    {
        // given
        int expectedNumberOfErrors = 3;
        bool expectedResultForIsSuccessProperty = false;
        bool expectedResultForHasErrorProperty = true;

        // when
        Result result = Result.Failures(
            [
                Error.Required("First name"),
                Error.Required("Last name"),
                Error.Required("Last name"),
                Error.WithMessage("Unknown error occurred")
            ]);

        // then
        result.IsSuccess.Should().Be(expectedResultForIsSuccessProperty);
        result.HasError.Should().Be(expectedResultForHasErrorProperty);
        result.Errors.Count.Should().Be(expectedNumberOfErrors);
    }
}