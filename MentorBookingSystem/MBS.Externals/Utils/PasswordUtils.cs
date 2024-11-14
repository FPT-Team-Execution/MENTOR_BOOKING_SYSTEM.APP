using System.Text;

namespace MBS.Externals.Utils;

public class PasswordUtils
{
        private static readonly char[] UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] LowercaseChars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static readonly char[] NumberChars = "0123456789".ToCharArray();
        private static readonly char[] SpecialChars = "!@".ToCharArray();

        public static string GenerateRandomPassword(int length = 12)
        {
            var random = new Random();
            var password = new StringBuilder();

            // Ensure the password includes at least one character from each category
            password.Append(UppercaseChars[random.Next(UppercaseChars.Length)]);
            password.Append(LowercaseChars[random.Next(LowercaseChars.Length)]);
            password.Append(NumberChars[random.Next(NumberChars.Length)]);
            password.Append(SpecialChars[random.Next(SpecialChars.Length)]);

            // Fill the rest of the password length with random characters
            var allChars = new char[UppercaseChars.Length + LowercaseChars.Length + NumberChars.Length +
                                    SpecialChars.Length];
            UppercaseChars.CopyTo(allChars, 0);
            LowercaseChars.CopyTo(allChars, UppercaseChars.Length);
            NumberChars.CopyTo(allChars, UppercaseChars.Length + LowercaseChars.Length);
            SpecialChars.CopyTo(allChars, UppercaseChars.Length + LowercaseChars.Length + NumberChars.Length);

            for (int i = password.Length; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the password to mix the characters
            var passwordArray = password.ToString().ToCharArray();
            for (int i = 0; i < passwordArray.Length; i++)
            {
                int j = random.Next(i, passwordArray.Length);
                var temp = passwordArray[i];
                passwordArray[i] = passwordArray[j];
                passwordArray[j] = temp;
            }

            return new string(passwordArray);
        }
}