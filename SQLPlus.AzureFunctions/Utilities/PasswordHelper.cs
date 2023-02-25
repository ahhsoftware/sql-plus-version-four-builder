using System;
using System.Linq;
using System.Security.Cryptography;

namespace SQLPlus.AzureFunctions.Utilities
{
    public class PasswordHelper
    {
        #region Properties

        public string? Password { get; private set; }
        public int? Iterations { get; private set; }
        public byte[]? Salt { get; private set; }
        public byte[]? Hash { get; private set; }

        #endregion Properties

        #region Constructors

        public PasswordHelper() { }

        #endregion Constructors

        /// <summary>
        /// This method will populate the Iterations, Salt, and Hash properties.
        /// </summary>
        /// <param name="password">The password to build values for.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void BuildValues(string password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            Salt = RandomNumberGenerator.GetBytes(16);
            Iterations = RandomNumberGenerator.GetInt32(1000);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, Salt, iterations: Iterations.Value);
            Hash = pbkdf2.GetBytes(16);
        }

        public bool IsMatch(string password, byte[] hash, byte[] salt, int iterations)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] test = pbkdf2.GetBytes(16);
            return test.SequenceEqual(hash);
        }

        public void GenerateRandomPassword()
        {
            Password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
            BuildValues(Password);
        }
    }
}
