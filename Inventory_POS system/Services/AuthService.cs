using Inventory_POS_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_POS_system.Services
{
    public static class AuthService
    {
        public static User CurrentUser { get; private set; }

        public static bool Login(string username, string password)
        {
            var users = JsonService.Load<List<User>>("users.json");
            if (users == null) return false;

            var user = users.FirstOrDefault(u =>
                u.UserName == username &&
                u.Password == password);

            if (user == null) return false;

            CurrentUser = user;
            return true;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsAdmin =>
            CurrentUser?.Role == "Admin";
    }
}
