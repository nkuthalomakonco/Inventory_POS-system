
using Inventory_POS_system.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Inventory_POS_system.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();

            var vm = new ViewModels.RegisterViewModel();
            DataContext = vm;

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        //{

        //}


    }
}
