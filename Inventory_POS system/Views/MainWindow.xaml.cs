using Inventory_POS_system.ViewModels;
using Inventory_POS_system.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace Inventory_POS_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainVM;

        public MainWindow()
        {
            InitializeComponent();
            _mainVM = new MainViewModel();
            DataContext = _mainVM;

            // Navigate to default page
            //MainFrame.Navigate(new InventoryView(_mainVM.InventoryVM));
            //MainFrame.Navigate(new POSView(_mainVM.POSVM));
            // Navigate to login page inside the frame
            MainFrame.Navigate(new LoginView());
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_mainVM.CanAccessInventory)
            {
                // Show login page instead
                MainFrame.Navigate(new LoginView());
            }
            else
            {
                // User already authorized
                MainFrame.Navigate(new InventoryView(_mainVM.InventoryVM));
            }
        }

        private void POSButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new POSView(_mainVM.POSVM));
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        // Minimize window
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Close window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}