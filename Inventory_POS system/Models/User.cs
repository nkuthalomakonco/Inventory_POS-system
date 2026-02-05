
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Inventory_POS_system.Models
{
    enum UserRole
    {
        Admin,
        Manager,
        Cashier
    }
    public class User
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public string Role { get; set; }     // Admin / Cashier
        public string PasswordHash { get; set; } // store hashed password
        public string Salt { get; set; }

    }

public static class PasswordHelper
    {
        private const int SaltSize = 16; // 128-bit
        private const int KeySize = 32;  // 256-bit
        private const int Iterations = 10000;

        public static (string hash, string salt) HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[SaltSize];
            rng.GetBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            using var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
            string hash = Convert.ToBase64String(deriveBytes.GetBytes(KeySize));

            return (hash, salt);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            using var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
            string hash = Convert.ToBase64String(deriveBytes.GetBytes(KeySize));
            return hash == storedHash;
        }
    }

}
