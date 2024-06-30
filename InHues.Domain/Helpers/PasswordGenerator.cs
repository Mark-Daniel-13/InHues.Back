using System;

namespace InHues.Domain.Helpers
{
    public class PasswordGenerator
    {
        private static readonly Random Random = new Random();

        public static string GenerateRandomPassword(int baseLength = 6)
        {
            const string alphaCharacters = "abcdefghijklmnopqrstuvwxyz";
            const string alphaBigCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numericCharacters = "0123456789";
            const string specialCharacters = "!@-.";

            var random = new Random();
            int lnd = random.Next(1, 5);
            int lbc = random.Next(1,5);
            int sc = random.Next(1, 3);
            int totalLength = baseLength + (lbc + lnd + sc);
            var password = new char[totalLength];

            // Ensure numeric digits
            for (var i = 0; i < lnd; i++)
            {
                password[i] = numericCharacters[random.Next(numericCharacters.Length)];
            }
            // Ensure big chars
            for (var i = lnd; i < (lbc+lnd); i++)
            {
                password[i] = alphaBigCharacters[random.Next(alphaBigCharacters.Length)];
            }
            // Ensure special chars
            for (var i = (lbc + lnd); i < (lbc+lnd+sc); i++)
            {
                password[i] = specialCharacters[random.Next(specialCharacters.Length)];
            }
            for (var i = (lbc + lnd + sc); i < totalLength; i++)
            {
                password[i] = alphaCharacters[random.Next(alphaCharacters.Length)];
            }

            // Shuffle the characters randomly
            for (var i = 0; i < totalLength; i++)
            {
                var j = random.Next(i, totalLength);
                var temp = password[i];
                password[i] = password[j];
                password[j] = temp;
            }

            return new string(password);
        }
    }
}
