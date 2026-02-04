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

        private string _error;
        public string ErrorMessage
        {
            get => _error;
            set { _error = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public event Action LoginSucceeded;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }
        private void Login()
        {
            if (AuthService.Login(Username, Password))
            {
                var mainWindow = App.Current.MainWindow as MainWindow;
                var mainVM = mainWindow?.DataContext as MainViewModel;

                mainVM?.RefreshPermissions();

                // Go to Inventory AFTER login
                mainWindow?.MainFrame.Navigate(
                    new Views.InventoryView(mainVM.InventoryVM)
                );
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }

    }

}
