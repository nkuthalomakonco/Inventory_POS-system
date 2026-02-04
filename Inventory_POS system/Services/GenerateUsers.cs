using Inventory_POS_system.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory_POS_system.Services
{
    class GenerateUsers
    {
        public GenerateUsers()
        {
            var users = new List<User>();

            // Create Admin user
            var (adminHash, adminSalt) = PasswordHelper.HashPassword("admin123");
            users.Add(new User
            {
                Username = "admin",
                PasswordHash = adminHash,
                Salt = adminSalt,
                Role = "Admin"
            });

            // Another Admin
            var (admin2Hash, admin2Salt) = PasswordHelper.HashPassword("1234");
            users.Add(new User
            {
                Username = "user",
                Password = admin2Hash,
                PasswordHash = admin2Hash,
                Salt = admin2Salt,
                Role = "Admin"
            });

            // Cashier
            var (cashierHash, cashierSalt) = PasswordHelper.HashPassword("2026");
            users.Add(new User
            {
                Username = "cashier",
                PasswordHash = cashierHash,
                Salt = cashierSalt,
                Role = "Cashier"
            });

            // Another Cashier
            var (meHash, meSalt) = PasswordHelper.HashPassword("2026");
            users.Add(new User
            {
                Username = "nkuthalo",
                PasswordHash = meHash,
                Salt = meSalt,
                Role = "Cashier"
            });

            // Save to JSON file
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("users.json", json);

            Console.WriteLine("Users.json created with secure hashed passwords!");
        }
    }
}
