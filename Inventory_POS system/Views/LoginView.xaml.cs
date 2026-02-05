using Inventory_POS_system.Services;
using Inventory_POS_system.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Inventory_POS_system.Views
{
    public partial class LoginView : Page
    {
        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
            UsernameTextBox.Text = "admin"; // Default username for testing
            PasswordBox.Password = "admin123"; // Default password for testing
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (AuthService.Login(username, password))
            {
                // Get MainViewModel
                var mainVM = (Application.Current.MainWindow.DataContext as MainViewModel);

                // Refresh Inventory access
                mainVM?.RefreshPermissions();

                // Optionally navigate to InventoryView automatically
                Application.Current.MainWindow?.Dispatcher.Invoke(() =>
                {
                    (Application.Current.MainWindow as MainWindow)
                        ?.MainFrame
                        .Navigate(new InventoryView(mainVM.InventoryVM));
                });
            }
            else
            {
                //MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Create and show the registration window as a modal popup
            var registerWindow = new RegisterView();
            registerWindow.Owner = Window.GetWindow(this); // sets login as owner
            registerWindow.ShowDialog();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = PasswordBox.Password;
        }

    }
}
