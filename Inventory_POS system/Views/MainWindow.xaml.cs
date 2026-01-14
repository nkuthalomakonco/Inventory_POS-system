using Inventory_POS_system.ViewModels;
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
            MainFrame.Navigate(new InventoryPage(_mainVM.InventoryVM));
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InventoryPage(_mainVM.InventoryVM));
        }

        private void POSButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new POSPage(_mainVM.POSVM));
        }
    }
}