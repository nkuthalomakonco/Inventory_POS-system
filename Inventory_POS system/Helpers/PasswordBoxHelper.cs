using System.Windows;
using System.Windows.Controls;

namespace Inventory_POS_system.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached(
                "Placeholder",
                typeof(string),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnPlaceholderChanged));

        public static string GetPlaceholder(DependencyObject obj) => (string)obj.GetValue(PlaceholderProperty);
        public static void SetPlaceholder(DependencyObject obj, string value) => obj.SetValue(PlaceholderProperty, value);

        private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
            {
                var placeholder = GetPlaceholderTextBlock(pb);
                if (placeholder != null)
                {
                    placeholder.Visibility = string.IsNullOrEmpty(pb.Password)
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
            }
        }

        private static TextBlock GetPlaceholderTextBlock(PasswordBox pb)
        {
            if (pb.Parent is Grid grid)
            {
                foreach (var child in grid.Children)
                {
                    if (child is TextBlock tb && tb.Tag?.ToString() == "Placeholder")
                        return tb;
                }
            }
            return null;
        }
    }
}
