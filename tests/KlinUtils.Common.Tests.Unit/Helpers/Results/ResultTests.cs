using KlinUtils.Common.Helpers.Results;

namespace KlinUtils.Common.Tests.Unit.Helpers.Results;

public class ResultTests
{
    [Fact]
    public void SuccessFactory_ValidValue_CreatesSuccessResult()
    {
        // Given
        string value = "hello";

        // When
        Result<string, Failure> result = Result<string, Failure>.Success(value);

        // Then
        Assert.False(result.HasError);
        Assert.Equal(value, result.SuccessValue);
    }

    [Fact]
    public void FailureFactory_ValidFailure_CreatesErrorResult()
    {
        // Given
        ErrorInfo errorInfo = ErrorInfo.BadRequest();
        Failure failure = new([errorInfo]);

        // When
        Result<string, Failure> result = Result<string, Failure>.Failure(failure);

        // Then
        Assert.True(result.HasError);
        Assert.Equal(failure, result.FailureValue);
    }

    [Fact]
    public void ImplicitConversion_FromSuccess_SetsSuccessValue()
    {
        // Given
        string value = "converted";

        // When
        Result<string, Failure> result = value;

        // Then
        Assert.False(result.HasError);
        Assert.Equal("converted", result.SuccessValue);
    }

    [Fact]
    public void ImplicitConversion_FromFailure_SetsFailureValue()
    {
        // Given
        ErrorInfo error = ErrorInfo.Conflict();
        Failure failure = new([error]);

        // When
        Result<string, Failure> result = failure;

        // Then
        Assert.True(result.HasError);
        Assert.Contains(error, result.FailureValue.Errors);
    }

    [Fact]
    public void FromTSuccess_ConstructsCorrectly()
    {
        // Given
        string value = "success";

        // When
        Result<string, Failure> result = Result<string, Failure>.FromTSuccess(value);

        // Then
        Assert.False(result.HasError);
        Assert.Equal("success", result.SuccessValue);
    }

    [Fact]
    public void FromTError_ConstructsCorrectly()
    {
        // Given
        ErrorInfo error = ErrorInfo.NotFound("Oops");
        Failure failure = new([error]);

        // When
        Result<string, Failure> result = Result<string, Failure>.FromTError(failure);

        // Then
        Assert.True(result.HasError);
        Assert.Contains(error, result.FailureValue.Errors);
    }

    [Fact]
    public void Match_SuccessBranch_ExecutesCorrectDelegate()
    {
        // Given
        Result<int, Failure> result = Result<int, Failure>.Success(42);

        // When
        string outcome = result.Match(
            success => $"Success:{success}",
            failure => "Error");

        // Then
        Assert.Equal("Success:42", outcome);
    }

    [Fact]
    public async Task MatchAsync_SuccessPath_ReturnsExpectedResult()
    {
        // Given
        Result<string, Failure> result = Result<string, Failure>.Success("async");

        // When
        string outcome = await result.Match(
            success => ValueTask.FromResult(success + "_ok"),
            failure => ValueTask.FromResult("failed"));

        // Then
        Assert.Equal("async_ok", outcome);
    }

    [Fact]
    public async Task MatchAsync_ErrorPath_ReturnsExpectedResult()
    {
        // Given
        Failure failure = new([ErrorInfo.InternalServerError()]);
        Result<string, Failure> result = Result<string, Failure>.Failure(failure);

        // When
        string outcome = await result.Match(
            success => ValueTask.FromResult("pass"),
            error => ValueTask.FromResult("fail"));

        // Then
        Assert.Equal("fail", outcome);
    }

    [Fact]
    public void SuccessValue_WhenIsFailure_ThrowsInvalidOperationException()
    {
        // Given
        Result<string, Failure> result = Result<string, Failure>.Failure(new Failure([ErrorInfo.Conflict()]));

        // When + Then
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
        {
            string unused = result.SuccessValue;
        });

        Assert.Contains("did not succeed", exception.Message, StringComparison.CurrentCultureIgnoreCase);
    }

    [Fact]
    public void FailureValue_WhenIsSuccess_ThrowsInvalidOperationException()
    {
        // Given
        Result<string, Failure> result = Result<string, Failure>.Success("ok");

        // When + Then
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
        {
            Failure unused = result.FailureValue;
        });

        Assert.Contains("succeed", exception.Message, StringComparison.CurrentCultureIgnoreCase);
    }

    [Fact]
    public void Equality_TwoSuccessResultsWithSameValue_AreEqual()
    {
        // Given
        Result<int, Failure> left = Result<int, Failure>.Success(10);
        Result<int, Failure> right = Result<int, Failure>.Success(10);

        // When
        bool equals = left == right;

        // Then
        Assert.True(equals);
    }

    [Fact]
    public void Equality_TwoDifferentResults_AreNotEqual()
    {
        // Given
        Result<int, Failure> left = Result<int, Failure>.Success(10);
        Result<int, Failure> right = Result<int, Failure>.Failure(new Failure([ErrorInfo.BadRequest()]));

        // When
        bool equals = left == right;

        // Then
        Assert.False(equals);
    }

    [Fact]
    public void GetHashCode_SameData_ProducesSameHash()
    {
        // Given
        Result<string, Failure> a = Result<string, Failure>.Success("ABC");
        Result<string, Failure> b = Result<string, Failure>.Success("ABC");

        // When
        int hashA = a.GetHashCode();
        int hashB = b.GetHashCode();

        // Then
        Assert.Equal(hashA, hashB);
    }
}