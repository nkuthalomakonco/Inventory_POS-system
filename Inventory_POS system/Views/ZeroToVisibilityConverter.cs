
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Inventory_POS_system.Views
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        // Converts an integer (like Password.Length) to Visibility
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int length && length == 0)
                return Visibility.Visible; // show placeholder
            return Visibility.Collapsed;    // hide placeholder
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
