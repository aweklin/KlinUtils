using KlinUtils.Common.Helpers;

namespace KlinUtils.Common.Tests.Unit.Helpers;

public class PasswordHelperTests
{
#pragma warning disable CA1859 // Use concrete types when possible for improved performance

    private readonly IPasswordHelper _passwordHelper;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

    public PasswordHelperTests()
    {
        _passwordHelper = new PasswordHelper();
    }

    [Fact]
    public void HashPassword_ValidInput_ReturnsExpectedHashFormat()
    {
        // Given
        string password = "Secure123!";

        // When
        string hash = _passwordHelper.HashPassword(password);

        // Then
        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.Equal(128, hash.Length);
        Assert.Matches("^[A-F0-9]{128}$", hash); // SHA-512 hash, uppercase hex
    }

    [Fact]
    public void HashPassword_ConsistentHashing_SameInputProducesSameOutput()
    {
        // Given
        string password = "StablePassword@2025";

        // When
        string hash1 = _passwordHelper.HashPassword(password);
        string hash2 = _passwordHelper.HashPassword(password);

        // Then
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void IsPasswordStrongEnough_NullOrWhitespace_ReturnsFalse()
    {
        // Given
        string? nullPassword = null;
        string whitespacePassword = "   ";

        // When
        bool result1 = _passwordHelper.IsPasswordStrongEnough(nullPassword!, 8);
        bool result2 = _passwordHelper.IsPasswordStrongEnough(whitespacePassword, 8);

        // Then
        Assert.False(result1);
        Assert.False(result2);
    }

    [Theory]
    [InlineData("password123!", false)] // Missing uppercase
    [InlineData("PASSWORD123!", false)] // Missing lowercase
    [InlineData("Password!!!", false)] // Missing number
    [InlineData("Password123", false)] // Missing special
    [InlineData("P@1a", false)] // Too short
    [InlineData("12345678!", false)] // Missing letters
    public void IsPasswordStrongEnough_InvalidInputs_ReturnsFalse(string password, bool expected)
    {
        // Given

        // When
        bool result = _passwordHelper.IsPasswordStrongEnough(password, 8);

        // Then
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsPasswordStrongEnough_ValidInput_ReturnsTrue()
    {
        // Given
        string password = "Strong1!Pass";

        // When
        bool result = _passwordHelper.IsPasswordStrongEnough(password, 8);

        // Then
        Assert.True(result);
    }

    [Fact]
    public void IsPasswordStrongEnough_CustomMinimumLength_TooShort_ReturnsFalse()
    {
        // Given
        string password = "A1b@";
        int minimumLength = 10;

        // When
        bool result = _passwordHelper.IsPasswordStrongEnough(password, minimumLength);

        // Then
        Assert.False(result);
    }

    [Fact]
    public void IsPasswordStrongEnough_CustomSpecialCharacters_InvalidSymbol_ReturnsFalse()
    {
        // Given
        string password = "Passw0rd~";
        string allowedSpecialCharacters = "!@#$";

        // When
        bool result = _passwordHelper.IsPasswordStrongEnough(password, 8, allowedSpecialCharacters);

        // Then
        Assert.False(result);
    }

    [Fact]
    public void IsPasswordStrongEnough_CustomSpecialCharacters_ValidSymbol_ReturnsTrue()
    {
        // Given
        string password = "Passw0rd$";
        string allowedSpecialCharacters = "!@#$";

        // When
        bool result = _passwordHelper.IsPasswordStrongEnough(password, 8, allowedSpecialCharacters);

        // Then
        Assert.True(result);
    }

    [Fact]
    public void IsPasswordStrongEnough_AllConditionsMet_ExactMinimumLength_ReturnsTrue()
    {
        // Given
        string password = "A1b!@#$%";
        int minimumLength = 8;

        // When
        bool result = _passwordHelper.IsPasswordStrongEnough(password, minimumLength);

        // Then
        Assert.True(result);
    }
}