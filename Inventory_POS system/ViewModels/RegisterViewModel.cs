using Inventory_POS_system.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;

namespace Inventory_POS_system.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        private string _role;
        public string Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(nameof(Role)); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        // Only one Roles property, properly initialized
        public ObservableCollection<string> Roles { get; } = new ObservableCollection<string>
        {
            "Admin",
            "User",
            "Guest"
        };

        public ICommand RegisterCommand { get; }

        private const string FilePath = "users.json";

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
        }

        private void Register()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Role))
            {
                ErrorMessage = "All fields are required!";
                return;
            }

            try
            {
                List<User> users = File.Exists(FilePath)
                    ? JsonSerializer.Deserialize<List<User>>(File.ReadAllText(FilePath)) ?? new List<User>()
                    : new List<User>();

                if (users.Exists(u => u.Username.Equals(Username, StringComparison.OrdinalIgnoreCase)))
                {
                    ErrorMessage = $"Username '{Username}' is already taken!";
                    return;
                }

                var (hash, salt) = PasswordHelper.HashPassword(Password);

                users.Add(new User
                {
                    Username = Username,
                    PasswordHash = hash,
                    Salt = salt,
                    Role = Role
                });

                File.WriteAllText(FilePath, JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));

                ErrorMessage = $"User '{Username}' registered successfully as '{Role}'!";

                Password = string.Empty;
                Role = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error saving user: " + ex.Message;
            }
        }
    }
}
