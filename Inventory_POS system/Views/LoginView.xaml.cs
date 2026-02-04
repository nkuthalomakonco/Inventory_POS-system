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
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
