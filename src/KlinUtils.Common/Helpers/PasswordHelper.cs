using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace KlinUtils.Common.Helpers;

internal sealed class PasswordHelper : IPasswordHelper
{
    public string HashPassword(string password)
    {
        byte[] data = Encoding.UTF8.GetBytes(password);
        byte[] hashedBytes = SHA512.HashData(data);

        StringBuilder hashedString = new(128);
        foreach (byte b in hashedBytes)
        {
            hashedString.Append(b.ToString("X2", CultureInfo.CurrentCulture));
        }

        return hashedString.ToString();
    }

    public bool IsPasswordStrongEnough(string password, int minimumLength, string allowedSpecialCharacters = "!@#$%^&*.")
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        bool containAlphabet = password.Any(char.IsLetter);
        bool containsNumber = password.Any(char.IsDigit);
        bool hasUpperCase = password.Any(char.IsUpper);
        bool hasLowerCase = password.Any(char.IsLower);
        bool hasSpecialCharacter = password.Any(allowedSpecialCharacters.Contains);
        bool meetsMinimumLengthRequired = password.Length >= minimumLength;

        return meetsMinimumLengthRequired &&
               containAlphabet &&
               containsNumber &&
               hasUpperCase &&
               hasLowerCase &&
               hasSpecialCharacter;
    }
}