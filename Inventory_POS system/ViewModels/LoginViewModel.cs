using System.ComponentModel;
using System.Windows.Input;
using Inventory_POS_system.Services;

namespace Inventory_POS_system.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password);

        private void Login()
        {
            // Replace this with real authentication
            if (Username == "admin" && Password == "password")
            {
                // Navigate to InventoryView
                //NavigationService.NavigateTo("InventoryView");
            }
            else
            {
                ErrorMessage = "Invalid username or password!";
            }
        }
    }
}
