using KlinUtils.Common.Helpers.Results;

namespace KlinUtils.Common.Tests.Unit.Helpers.Results;

public class ErrorInfoTests
{
    [Fact]
    public void BadRequest_DefaultMessage_ReturnsExpectedValues()
    {
        // Given + When
        ErrorInfo error = ErrorInfo.BadRequest();

        // Then
        Assert.Equal("BadRequest", error.Code);
        Assert.Equal("Invalid request", error.Message);
    }

    [Fact]
    public void Conflict_WithCustomMessage_SetsPropertiesCorrectly()
    {
        // Given
        string message = "This record already exists";

        // When
        ErrorInfo error = ErrorInfo.Conflict(message);

        // Then
        Assert.Equal("Conflict", error.Code);
        Assert.Equal(message, error.Message);
    }

    [Theory]
    [InlineData("Unauthorized", "Access denied")]
    [InlineData("Forbidden", "Forbidden")]
    [InlineData("InternalServerError", "Internal server error")]
    public void StaticFactories_ProduceExpectedDefaults(string expectedCode, string expectedMessage)
    {
        // Given + When
        ErrorInfo error = expectedCode switch
        {
            "Unauthorized" => ErrorInfo.Unauthorized(),
            "Forbidden" => ErrorInfo.Forbidden(),
            "InternalServerError" => ErrorInfo.InternalServerError(),
            _ => throw new NotImplementedException(),
        };

        // Then
        Assert.Equal(expectedCode, error.Code);
        Assert.Equal(expectedMessage, error.Message);
    }

    [Fact]
    public void Constructor_WithCustomCodeAndMessage_SetsFields()
    {
        // Given + When
        ErrorInfo error = new("Custom", "Something specific went wrong");

        // Then
        Assert.Equal("Custom", error.Code);
        Assert.Equal("Something specific went wrong", error.Message);
    }
}