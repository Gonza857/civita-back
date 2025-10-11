using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CivitaBack.Logica.Helpers
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 100_000; // más iteraciones = más lento = más seguro

        public static string HashPassword(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA256);

            var key = algorithm.GetBytes(KeySize);
            var salt = algorithm.Salt;

            // Guardamos juntos el salt y el hash, separados por |
            return $"{Convert.ToBase64String(salt)}|{Convert.ToBase64String(key)}";
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('|');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var key = Convert.FromBase64String(parts[1]);

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);

            var keyToCheck = algorithm.GetBytes(KeySize);

            return keyToCheck.SequenceEqual(key);
        }
    }
}
