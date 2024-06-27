using System;

namespace InHues.Domain.Helpers
{
    public class PasswordGenerator
    {
        private static readonly Random Random = new Random();

        public static string GenerateRandomPassword(int minLength = 6)
        {
            const string alphanumericCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var password = new char[minLength];
            for (var i = 0; i < minLength; i++)
            {
                password[i] = alphanumericCharacters[Random.Next(alphanumericCharacters.Length)];
            }
            return new string(password);
        }
    }
}
