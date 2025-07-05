namespace KlinUtils.Common.Helpers;

/// <summary>
/// Provides utilities for hashing and validating password strength.
/// </summary>
public interface IPasswordHelper
{
    /// <summary>
    /// Hashes a password using SHA-512 and returns the hash as an uppercase hexadecimal string.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>A SHA-512 hash string representation of the password.</returns>
    string HashPassword(string password);

#pragma warning disable CS1570 // XML comment has badly formed XML

    /// <summary>
    /// Determines whether the given password meets strength criteria including length, case, digit, and special character rules.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <param name="minimumLength">The minimum length required.</param>
    /// <param name="allowedSpecialCharacters">The set of allowed special characters (defaults to "!@#$%^&*.").</param>
    /// <returns><c>true</c> if the password is strong enough; otherwise, <c>false</c>.</returns>
    bool IsPasswordStrongEnough(string password, int minimumLength, string allowedSpecialCharacters = "!@#$%^&*.");
#pragma warning restore CS1570 // XML comment has badly formed XML

}